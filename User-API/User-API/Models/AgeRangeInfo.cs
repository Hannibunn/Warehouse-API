using Microsoft.EntityFrameworkCore;

namespace User_API.Models
{
    [Owned]
    public class AgeRangeInfo
    {
 
        public int? Id { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
    }
}
