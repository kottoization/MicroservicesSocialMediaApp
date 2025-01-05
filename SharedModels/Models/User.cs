using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SharedModels.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Relacja 1:N z Post:
        public ICollection<Post> Posts { get; set; }
        // Relacja 1:N z Comment:
        public ICollection<Comment> Comments { get; set; }
    }
}
