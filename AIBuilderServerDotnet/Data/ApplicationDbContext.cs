using Microsoft.EntityFrameworkCore;
using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Widget> Widgets { get; set; }
    }

}
