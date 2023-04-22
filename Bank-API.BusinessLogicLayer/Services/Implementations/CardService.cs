namespace Bank_API.BusinessLogicLayer.Services.Implementations;

public class CardService : ICardService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration configuration;

    public CardService(IUnitOfWork unitOfWork,
                       IConfiguration configuration)
    {
        this.unitOfWork = unitOfWork;
        this.configuration = configuration;
    }

    public async Task<APIResponse<int>> CreateCard(CardCreateDTO cardRequest, int userId)
    {
        APIResponse<int> response = new ();
        int sameCurrencyCardsCount = await unitOfWork.CardRepository
            .GetCount(x => x.UserId == userId && x.Currency == cardRequest.Currency);

        if (sameCurrencyCardsCount < 2)
        {
            Card newCard = new ()
            {
                UserId = userId,
                Balance = 0,
                Cvv = Convert.ToInt16(new Random().Next(1000).ToString("000")),
                Number = await CreateCardNumber(),
                Exp = DateTime.Now.AddYears(6),
                Currency = cardRequest.Currency,
                Status = CardStatus.active,
            };

            await unitOfWork.CardRepository.Create(newCard);
            await unitOfWork.Save();

            response.Result = newCard.Id;
            return response;
        }

        response.ErrorMessage = "Card not created. You can have only 2 cards of same currency.";

        return response;
    }

    public async Task<ICollection<CardDTO>> GetUserCards(int userId, CancellationToken cancellationToken)
    {
        return await unitOfWork.CardRepository.GetWhereSelectAll(
            x => x.UserId == userId,
            c => new CardDTO
            {
                Id = c.Id,
                Number = c.Number,
                Exp = c.Exp.ToString("MM/yy", CultureInfo.InvariantCulture),
                Cvv = c.Cvv,
                Currency = c.Currency.ToString(),
                Balance = c.Balance,
                Status = c.Status.ToString(),
            }, 
            cancellationToken); 
    }

    public async Task<APIResponse<bool>> ChangeCardStatus(bool freezeCard, int cardId)
    {
        APIResponse<bool> response = new();
        bool cardExists = await unitOfWork.CardRepository.Any(x => x.Id == cardId);

        if (!cardExists) 
        {
            response.ErrorMessage = "Card not found or unavailable";

            return response;
        }

        CardStatus cardStatus = await unitOfWork.CardRepository
            .GetFirstOrDefaultWithSelect(x => x.Id == cardId, x => x.Status);

        CardStatus requiredStatus = 
            freezeCard != true 
            ? CardStatus.active 
            : CardStatus.frozen;

        if (cardStatus != requiredStatus)
        {
            cardStatus = requiredStatus;
            await unitOfWork.CardRepository.UpdateCardStatus(x => x.Id == cardId, cardStatus);
            response.Result = true;

            return response;
        }

        response.ErrorMessage = "Card Is already frozen/unfrozen";

        return response;
    }

    private async Task<long> CreateCardNumber()
    {
        long number = long.Parse(configuration["Card:GeneratorFirstNumber"]!);
        Card? card = await unitOfWork.CardRepository.GetLastOrDefault();

        return card != null ?
            card.Number! + 1
            : number;
    }
}