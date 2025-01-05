﻿using SharedModels.Models;

public class Comment
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }
   
    public string UserId { get; set; }
    public User User { get; set; }

    public string UserName { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
