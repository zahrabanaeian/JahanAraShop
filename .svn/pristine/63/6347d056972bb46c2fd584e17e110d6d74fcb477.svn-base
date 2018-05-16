using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace JahanAraShop.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=DataBaseContext", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>()
                    .ToTable("tblSiteUsers", "dbo")
                    .Property(p => p.Id)
                    .HasColumnName("User_Id");
            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable("tblSiteUsersLogin", "dbo")
                .HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>()
                .ToTable("tblSiteRoles", "dbo")
                .HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>()
                .ToTable("tblSiteUserRoles", "dbo")
                .HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<IdentityUserClaim>()
           .ToTable("tblSiteUserClaims");
            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable("tblSiteUserLogins");
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}