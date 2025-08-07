using Microsoft.EntityFrameworkCore;
using User_API.Data;
using User_API.Models;

namespace User_API.Services
{
    public class BoxService : IBoxService
    {
        private readonly ApplicatonDbContext _context;

        public BoxService(ApplicatonDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Box>> GetBoxesByUserIdAsync(int userId)
        {
            return await _context.Boxes
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
