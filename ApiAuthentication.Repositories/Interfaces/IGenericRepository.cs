namespace ApiAuthentication.Repositories.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task<T?> GetRecordById(Guid id);
    Task AddRecord(T entity);
    void UpdateRecord(T entity);
    Task<bool> SaveChangesAsync();
}
