using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Enums;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository<Card> cardRepository;
        private readonly IConfiguration configuration;
        private readonly IAuthService authService;

        public CardService(ICardRepository<Card> cardRepository, 
                           IAuthService authService,                          
                           IConfiguration configuration,
                           IAuthService authService1)
        {
            this.cardRepository = cardRepository;
            this.configuration = configuration;
            this.authService = authService;
        }

        public async Task<int?> CreateCard(CardCreateRequest cardRequest)
        {
            var user = await authService.GetUser();
            var sameCurrencyCards = await cardRepository.GetUserCards(user!.Id, (Currency)Enum.Parse(typeof(Currency), cardRequest.Currency!));

            if(user != null && sameCurrencyCards!.Count < 2)
            {
                var newCard = new Card
                {
                    UserId = user.Id,
                    Balance = 0,
                    Cvv = Convert.ToInt16(new Random().Next(1000).ToString("000")),
                    Number = await GetNumber(),
                    Exp = DateTime.Now.AddYears(6),
                    Currency = (Currency) Enum.Parse(typeof(Currency), cardRequest.Currency!),
                    Status = CardStatus.active,
                };

                await cardRepository.CreateCard(newCard);
                return newCard.Id;
            }

            return null;
        }

        public async Task<CardResponse[]?> GetUserCards()
        {
            var user = await authService.GetUser();
            var cards = await cardRepository.GetUserCardsById(user!.Id);

            if (cards != null)
            {
                var arraysResponse = cards.Select(c => new CardResponse
                {
                    Id = c.Id,
                    Number = c.Number,
                    Exp = c.Exp?.ToString("MM/yy", CultureInfo.InvariantCulture),
                    Cvv = c.Cvv,
                    Currency = c.Currency.ToString(),
                    Balance = c.Balance,
                    Status = c.Status.ToString(),
                })
                    .ToArray();

                return arraysResponse;
            }

            return null;
        }

        public async Task<bool> ChangeCardStatus(int id)
        {
            var user = await authService.GetUser();
            var cards = await cardRepository.GetUserCardsById(user!.Id);

            if(cards != null)
            {
                var card = cards!.FirstOrDefault(c => c.Id == id);

                //Âüובאעü switch-case גלוסעמ if-מג

                if (card!.Status != CardStatus.frozen)
                {
                    card.Status = CardStatus.frozen;
                    return true;
                }

                if (card.Status == CardStatus.frozen)
                {
                    card.Status = CardStatus.active;
                    return true;
                }
            }

            return false;
        }

        private async Task<long?> GetNumber()
        {
            long number = long.Parse(configuration["Card:GeneratorFirstNumber"]);
            var card = await cardRepository.GetLastCard();

            return card != null ?
                (card!.Number! + 1)
                : number;
        }
    }
}


