namespace User_API.Models
{
    public class Box
    {
        public int Id { get; set; }

        public string Label { get; set; } = string.Empty;

        // Optional: Welchem User gehört die Box? (falls du das speichern willst)
        public int UserId { get; set; }
        public Users User { get; set; }

        // Navigation: Welche Sets sind in der Box?
        public ICollection<UserSet> UserSets { get; set; } = new List<UserSet>();
    }
}
