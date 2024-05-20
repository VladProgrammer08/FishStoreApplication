using FishStoreApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FishStoreApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fish> Fishes { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
    }
}
