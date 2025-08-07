using User_API.Models;

namespace User_API.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(Users user);
        Task<Users?> GetUserByEmailAsync(string email);
    }
}
