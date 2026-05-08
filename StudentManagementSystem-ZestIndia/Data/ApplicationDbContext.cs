using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_ZestIndia.Models;

namespace StudentManagementSystem_ZestIndia.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
