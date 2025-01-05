using System;

namespace PostAPI.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; } // Nowe pole

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
