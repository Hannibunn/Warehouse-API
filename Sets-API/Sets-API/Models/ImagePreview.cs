using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sets_API.Models
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    /// <remarks>
    /// This class represents an image associated with a particular entity.
    /// </remarks>
    [NotMapped]
    public class ImagePreview
    {
        /// <summary>
        /// Gets or sets the unique identifier of the image.
        /// </summary>
        [Key]
        public int ID { get; set; }
        public string? ThumbnailURL { get; set; }
        public string? ImageURL { get; set; }
    }
}
