using System;
using System.Collections.Generic;

namespace SharedModels.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public User User { get; set; }
    }
}
