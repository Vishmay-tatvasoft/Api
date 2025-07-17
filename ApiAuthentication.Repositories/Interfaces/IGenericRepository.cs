namespace ApiAuthentication.Repositories.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task<T> GetRecordById(string id);
}
