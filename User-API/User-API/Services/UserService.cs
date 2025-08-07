using User_API.Data;
using User_API.Models;
using Microsoft.EntityFrameworkCore;


namespace User_API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicatonDbContext _context;
        private readonly IAuthService _authService;

        public UserService(ApplicatonDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<bool> RegisterUserAsync(Users user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            user.Password = _authService.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
