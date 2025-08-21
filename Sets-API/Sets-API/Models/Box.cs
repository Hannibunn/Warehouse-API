using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sets_API.Models
{
    public class Box
    {
        /// <summary>
        /// Gets or sets the unique identifier of the box.
        /// </summary>
        [Key]
        public int ID { get; set; }

        public int? StorageID { get; set; }

        /// <summary>
        /// Gets or sets the storage of the box
        /// </summary>
        public Storage? Storage { get; set; }

        /// <summary>
        /// Gets or sets the list of sets stored in the box
        /// </summary>
        public ICollection<Set>? Sets { get; set; } = new List<Set>();

        /// <summary>
        /// Gets or sets the name of the box.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the box.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the QR-ode associated with the box.
        /// </summary>
        public string? QRcode { get; set; }
        // Box gehört einem User
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
