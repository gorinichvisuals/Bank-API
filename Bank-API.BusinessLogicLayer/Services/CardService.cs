using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Enums;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Bank_API.DataAccessLayer.Repositories;
using Microsoft.Extensions.Configuration;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository<Card> cardRepository;
        private readonly IAuthService authService;
        private readonly IUserRepository<User> userRepository;
        private readonly IConfiguration configuration;

        public CardService(ICardRepository<Card> cardRepository, 
                           IAuthService authService,
                           IUserRepository<User> userRepository,
                           IConfiguration configuration)
        {
            this.cardRepository = cardRepository;
            this.authService = authService;
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public async Task<int?> CreateCard(CardCreateRequest cardRequest)
        {
            var authenticateUser = authService.GetAuthenticateUser();
            var user = await userRepository.GetUserByEmail(authenticateUser!.Email!);
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

        public async Task<CardResponce[]?> GetUserCards()
        {
            var authenticateUser = authService.GetAuthenticateUser();
            var user = await userRepository.GetUserByEmail(authenticateUser!.Email!);
            var cards = await cardRepository.GetUserCardsById(user!.Id);

            if (cards != null)
            {
                CardResponce[] cardsResponce = new CardResponce[cards.Length];

                for (int i = 0; i < cards.Length; i++)
                {
                    var cardResponce = new CardResponce
                    {
                        Id = cards[i].Id,
                        Number = cards[i].Number,
                        Exp = String.Format("{0:MM/yy}", cards[i].Exp),
                        Cvv = cards[i].Cvv,
                        Currency = cards[i].Currency.ToString(),
                        Balance = cards[i].Balance,
                        Status = cards[i].Status.ToString(),
                    };

                    cardsResponce[i] = cardResponce;
                }

                return cardsResponce;
            }

            return null;
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
