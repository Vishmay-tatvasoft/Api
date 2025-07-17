using ApiAuthentication.Entity.Models;

namespace ApiAuthentication.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> ValidateCredentials(string email, string password);
}
