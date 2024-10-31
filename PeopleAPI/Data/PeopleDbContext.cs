using PeopleAPI.Models;
using System.Data.Entity;

namespace PeopleAPI.Data
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
