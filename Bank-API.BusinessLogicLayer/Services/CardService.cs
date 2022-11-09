using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Enums;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository<Card> cardRepository;
        private readonly IAuthService authService;
        private readonly IUserRepository<User> userRepository;

        public CardService(ICardRepository<Card> cardRepository, 
                           IAuthService authService,
                           IUserRepository<User> userRepository)
        {
            this.cardRepository = cardRepository;
            this.authService = authService;
            this.userRepository = userRepository;
        }

        public async Task<int?> CreateCard(CardCreateRequest cardRequest)
        {
            var authenticateUser = authService.GetAuthenticateUser();
            var user = await userRepository.GetUserByEmail(authenticateUser!.Email!);

            if(user != null && user.Cards!.Count < 2)
            {
                var newCard = new Card
                {
                    UserId = user.Id,
                    Balance = 0,
                    Cvv = Convert.ToInt16(new Random().Next(1000).ToString("000")),
                    Number = await GetNumber(),
                    Exp = DateTime.Now.AddYears(6),
                    Currency = (Currency) Enum.Parse(typeof(Currency), cardRequest.Currency!),
                    Status = DataAccessLayer.Enums.CardStatus.Active,
                };

                await cardRepository.CreateCard(newCard);
                return newCard.Id;
            }

            return null;
        }

        private async Task<long?> GetNumber()
        {
            long start = 4141562365230001;
            long count = 1;
            var card = await cardRepository.GetLastCard();

            card!.Number =
                 card.Number != null 
                ? card.Number + count 
                : start;

            return card.Number;
        }
    }
}
