using Microsoft.EntityFrameworkCore;
using CodePulse.Models.Domain;

namespace CodePulse.Data
{
  public class ApplicationDBContext: DbContext  
  {
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
    { 
    }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BlogImages> BlogImages { get; set; }
      
  }
}
