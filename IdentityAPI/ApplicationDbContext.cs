using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace IdentityAPI
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji między Post a User
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)            
                .WithMany()                     
                .HasForeignKey(p => p.UserId)   
                .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
