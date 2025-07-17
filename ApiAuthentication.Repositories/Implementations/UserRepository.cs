using ApiAuthentication.Entity.Data;
using ApiAuthentication.Entity.Models;
using ApiAuthentication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Repositories.Implementations;

public class UserRepository(DemoWebApiContext context) : IUserRepository
{
    private readonly DemoWebApiContext _context = context;

    #region Validate Credentials
    public async Task<User> ValidateCredentials(string email, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.HashPassword == password);
    }
    #endregion

}
