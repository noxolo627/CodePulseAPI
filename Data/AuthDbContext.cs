using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Data
{
  public class AuthDbContext : IdentityDbContext
  {
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      //create a reader and writer roles

      var readerRoleId = "812d20d7-6440-4641-aec8-d61d02dbe58d";
      var writerRoleId = "c73ff95c-e4d9-4eba-ac1c-64e8a1a1ff7f";

      var roles = new List<IdentityRole>
      {
        new IdentityRole(){
         Id = readerRoleId,
         Name = "Reader",
         NormalizedName = "Reader".ToUpper(),
         ConcurrencyStamp = readerRoleId
        },
        new IdentityRole()
        {
         Id = writerRoleId,
         Name = "Writer",
         NormalizedName = "Writer".ToUpper(),
         ConcurrencyStamp = writerRoleId
        }
      };

      //seed the roles to the database
      builder.Entity<IdentityRole>().HasData(roles);

      //create an Admin user

      var adminUserId = "799ac999-ddf2-47ed-af24-19c8921cde17";

      var admin = new IdentityUser
      {
        Id = adminUserId,
        UserName = "noxy627",
        Email = "noxy627@gmail.com",
        NormalizedEmail = "noxy627@gmail.com".ToUpper(),
        NormalizedUserName = "noxy627@gmail.com".ToUpper(),

      };

      admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "noxy101");

      builder.Entity<IdentityUser>().HasData(admin);

      //Give roles to the user
      var adminRoles = new List<IdentityUserRole<string>>()
      {
        new()
          {
            UserId = adminUserId,
            RoleId = readerRoleId
        },
        new()
        {
          UserId = adminUserId,
          RoleId = writerRoleId
        }
      };

      builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
  }
}
