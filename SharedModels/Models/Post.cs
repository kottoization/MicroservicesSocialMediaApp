using System;
using System.Collections.Generic;

namespace SharedModels.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        // Dodajemy w�a�ciwo�� nawigacyjn� do obiektu User:
        public User User { get; set; }

        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Dodajemy w�a�ciwo�� nawigacyjn� do powi�zanych komentarzy:
        public ICollection<Comment> Comments { get; set; }
    }
}
