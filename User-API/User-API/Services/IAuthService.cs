namespace User_API.Services
{
    public interface IAuthService
    {
        int? GetUserIdFromToken(HttpContext context);
        string GenerateJwtToken(string email);
        string HashPassword(string password);      // Neu hinzufügen
        bool VerifyPassword(string password, string hashedPassword); // Optional, falls du das brauchst
    }
}
