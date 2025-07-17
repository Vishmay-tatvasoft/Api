using ApiAuthentication.Entity.Data;
using ApiAuthentication.Repositories.Interfaces;

namespace ApiAuthentication.Repositories.Implementations;

public class GenericRepository<T>(DemoWebApiContext context) : IGenericRepository<T>
    where T : class
{
    private readonly DemoWebApiContext _context = context;

    #region Get Record By ID
    public async Task<T> GetRecordById(string id){
        return await _context.Set<T>().FindAsync(id);
    }
    #endregion

    

}
