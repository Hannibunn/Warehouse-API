using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Warehouse_API_Test.Migration
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string email { get; set; } = null!;
        [Required]
        public string HashedPassword { get; set; } = null!; 
    }
}
