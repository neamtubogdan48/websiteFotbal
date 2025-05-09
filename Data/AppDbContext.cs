using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvc.data.models;
using mvc.Models;

namespace mvc.Data
{
    // Update the AppDbContext class to inherit from IdentityDbContext<Users>
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Sponsor> Sponsor { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Contact> Contact { get; set; }
    }
}
