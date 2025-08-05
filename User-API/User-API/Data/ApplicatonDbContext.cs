using Microsoft.EntityFrameworkCore;

namespace User_API.Data
{
    public class ApplicatonDbContext: DbContext
    {
        public ApplicatonDbContext(DbContextOptions<ApplicatonDbContext> options)
          : base(options)
        {

        }
    }
}
