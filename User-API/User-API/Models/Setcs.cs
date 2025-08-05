namespace User_API.Models
{
    public class Setcs
    {
        public int UserId { get; set; }
        public Users User { get; set; }

        public int SetId { get; set; }
        public LegoSet Set { get; set; }

        // Optional: In welcher Box wird dieses Set gelagert?
        public int? BoxId { get; set; }
        public Box? Box { get; set; }
    }
}
