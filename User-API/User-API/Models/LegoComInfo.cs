using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace User_API.Models
{
    public class LegoComInfo
    {
        [Key]
        public int Id { get; set; }
        public RegionInfo? US { get; set; }
        public RegionInfo? UK { get; set; }
        public RegionInfo? CA { get; set; }
        public RegionInfo? DE { get; set; }
    }
}
