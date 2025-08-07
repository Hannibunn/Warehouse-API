using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace User_API.Models
{
    public class Box
    {
        [Key]
        public int ID { get; set; }

        public int? StorageID { get; set; }
        public Storage? Storage { get; set; }

        public ICollection<Set>? Sets { get; set; } = new List<Set>();

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? QRcode { get; set; }

        // Box gehört einem User
        public int UserId { get; set; }
        public Users User { get; set; }   
    }
}
