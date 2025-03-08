using Code_Orbit.Models;
using Microsoft.EntityFrameworkCore;

namespace Code_Orbit.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }

}
