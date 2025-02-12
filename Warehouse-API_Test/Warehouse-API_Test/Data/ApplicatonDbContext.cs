using Microsoft.EntityFrameworkCore;
using System;
using Warehouse_API_Test.Models;

namespace Warehouse_API_Test.Data
{
    public class ApplicatonDbContext:DbContext
    {
     public DbSet<Users> Users { get; set; }
        public ApplicatonDbContext(DbContextOptions<ApplicatonDbContext> options)
            :base(options)
        {
            
        }
    }
}
