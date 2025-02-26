using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private ApplicationDbContext applicationDbContext;

        // Constructor mặc định
        public ApplicationUser() { }

        public ApplicationUser(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public string Fullname { get; set; }
        public string Phone { get; set; }
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
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Delivery>deliveries { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderDetail> orderDetails { get; set; }

        public DbSet<Post> posts { get; set; }

        public DbSet<PostDetail> postDetails { get; set; }

        public DbSet<PostCategory> postCategories { get; set; }

        public DbSet<Product> products { get; set; }

        public DbSet<ProductCategory> productCategories { get; set; }

        public DbSet<Slider> sliders  { get; set; }
        public DbSet<Sale> sales { get; set; }
        public DbSet<Status> statuses { get; set; }
    public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}