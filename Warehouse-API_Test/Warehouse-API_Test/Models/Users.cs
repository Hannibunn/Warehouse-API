using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Warehouse_API_Test.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string email { get; set; } =null!;
        [Required]
        public string HashedPassword { get; set; } = null!;
    }
}
