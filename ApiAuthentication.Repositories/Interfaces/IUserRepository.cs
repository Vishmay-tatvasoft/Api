using ApiAuthentication.Entity.Models;

namespace ApiAuthentication.Repositories.Interfaces;

public interface IUserRepository
{
    Task<FhUser?> GetUserByUsername(string userName);
}
