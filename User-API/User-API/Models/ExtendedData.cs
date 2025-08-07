using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace User_API.Models
{
    [Owned]
    public class ExtendedData
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the list of the tags.
        /// </summary>
        public List<string> BrickTags { get; set; }
    }
}
