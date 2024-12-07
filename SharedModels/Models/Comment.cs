﻿namespace SharedModels.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Post Post { get; set; }
        //public User User { get; set; }
    }
}
