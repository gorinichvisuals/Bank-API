namespace Bank_API.DataAccessLayer.Repositories.Abstractions;

public interface IBaseRepository<T>
{
    public Task Create(T entity);

    public void Update(T entity);

    public Task<T?> GetFirstOrDefault(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    public Task<T?> GetLastOrDefault(CancellationToken cancellationToken = default);

    public Task<TResult?> GetFirstOrDefaultWithSelect<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> select,
        CancellationToken cancellationToken = default);

    public Task<ICollection<T>> GetAll(CancellationToken cancellationToken = default);

    public Task<ICollection<T>> GetWhereAll(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    public Task<ICollection<TResult>> GetWhereSelectAll<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> select,
        CancellationToken cancellationToken = default);

    public Task<bool> Any(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    public Task<int> GetCount(Expression<Func<T, bool>> predicate);
}