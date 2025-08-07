using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sets_API.Models
{
    [NotMapped]
    public class Dimensions
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// Gets or sets the height of the set.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the set.
        /// </summary>

        public double? Width { get; set; }

        /// <summary>
        /// Gets or sets the depth of the set.
        /// </summary>

        public double? Depth { get; set; }

        /// <summary>
        /// Gets or sets the weight of the set.
        /// </summary>
        public double? Weight { get; set; }
    }
}
