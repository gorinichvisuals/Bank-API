namespace Bank_API.DataAccessLayer.Repositories.Implementations;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly BankAPIContext context;

    public BaseRepository(BankAPIContext context)
    {
        this.context = context;
    }

    public async Task Create(T entity)
    {
        await context.Set<T>()
            .AddAsync(entity);
    }

    public void Update(T entity)
    {
        context.Set<T>()
            .Update(entity);
    }

    public async Task<T?> GetFirstOrDefault(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<T?> GetLastOrDefault(CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .LastOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetFirstOrDefaultWithSelect<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> select,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .Where(predicate)
            .Select(select)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<T>> GetAll(CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<T>> GetWhereAll(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<TResult>> GetWhereSelectAll<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> select,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .Where(predicate)
            .Select(select)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCount(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .CountAsync(predicate);
    }

    public async Task<bool> Any(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AnyAsync(predicate, cancellationToken);
    }
}