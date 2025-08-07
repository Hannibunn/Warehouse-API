namespace User_API.Models
{
    public class Users
    {
        public int? Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public ICollection<Box> Boxes { get; set; } = new List<Box>();
    }
}
