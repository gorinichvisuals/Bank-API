namespace Bank_API.DataAccessLayer.Repositories.Implementations;

internal sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(BankAPIContext context): base(context) 
    { 
    }
}