using ApiAuthentication.Entity.Data;
using ApiAuthentication.Entity.Models;
using ApiAuthentication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Repositories.Implementations;

public class UserRepository(TatvasoftFhContext context) : IUserRepository
{
    private readonly TatvasoftFhContext _context = context;

    #region Get User By Email
    public async Task<FhUser?> GetUserByEmail(string email)
    {
        return await _context.FhUsers.FirstOrDefaultAsync(u => u.EmailAddress == email);
    }
    #endregion

}
