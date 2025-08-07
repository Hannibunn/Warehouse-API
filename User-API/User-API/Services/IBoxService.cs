using User_API.Models;

namespace User_API.Services
{
    public interface IBoxService
    {
        Task<IEnumerable<Box>> GetBoxesByUserIdAsync(int userId);
    }
}
