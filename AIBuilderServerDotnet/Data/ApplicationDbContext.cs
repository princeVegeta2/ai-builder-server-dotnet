using Microsoft.EntityFrameworkCore;
using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }

}
