using ApiAuthentication.Entity.Data;
using ApiAuthentication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    protected readonly DemoWebApiContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(DemoWebApiContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    #region Get Record By ID
    public async Task<T> GetRecordById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }
    #endregion

    #region Add Record
    public async Task AddRecord(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    #endregion

    #region Update Record
    public async void UpdateRecord(T entity)
    {
        _dbSet.Update(entity);
    }
    #endregion   

    #region Save Changes To Db
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    #endregion 

}
