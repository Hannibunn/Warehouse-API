using System.ComponentModel.DataAnnotations;

namespace Sets_API.Models
{
    public class Storage
    {
        /// <summary>
        /// Gets or sets the unique identifier of the storage unit.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the storage unit.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the storage unit.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the ImageSource of the storage unit.
        /// </summary>
        public string? ImageSource { get; set; }

        /// <summary>
        /// Gets or sets the list of boxes stored in that storage
        /// </summary>
        public ICollection<Box>? Boxes { get; set; } = new List<Box>();
    }
}
