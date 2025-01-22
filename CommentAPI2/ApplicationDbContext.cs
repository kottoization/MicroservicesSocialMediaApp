using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace CommentAPI2
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Relacja Comment -> Post bez kaskadowego usuwania
        //    modelBuilder.Entity<Comment>()
        //        .HasOne<Post>()
        //        .WithMany(p => p.Comments)
        //        .HasForeignKey(c => c.PostId)
        //        .OnDelete(DeleteBehavior.Restrict); // Zamiast Cascade

        //    // Relacja Comment -> User bez kaskadowego usuwania
        //    modelBuilder.Entity<Comment>()
        //        .HasOne<User>()
        //        .WithMany(u => u.Comments)
        //        .HasForeignKey(c => c.UserId)
        //        .OnDelete(DeleteBehavior.Restrict); // Zamiast Cascade
        //}

    }
}


