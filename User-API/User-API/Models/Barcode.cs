using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User_API.Models
{
    [NotMapped]
    public class Barcode
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// Gets or sets the EAN (European Article Number) of the barcode.
        /// </summary>
        /// 
        public string? EAN { get; set; }

        /// <summary>
        /// Gets or sets the UPC (Universal Product Code) of the barcode.
        /// </summary>
        public string? UPC { get; set; }
    }
}
