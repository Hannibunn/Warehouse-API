using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User_API.Models
{
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
