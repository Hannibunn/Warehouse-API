using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace User_API.Models
{
    [Owned]

    public class RegionInfo
    {
        [Key]
        public int Id { get; set; }
        public double? RetailPrice { get; set; }
        public DateTime? DateFirstAvailable { get; set; }
        public DateTime? DateLastAvailable { get; set; }
    }
}
