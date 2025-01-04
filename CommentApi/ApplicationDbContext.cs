using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CommentApi
{
    namespace IdentityAPI
    {
        public class ApplicationDbContext : IdentityDbContext<User>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<Comment> Comments { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
        }
    }

}
