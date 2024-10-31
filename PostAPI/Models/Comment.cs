namespace PostAPI.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; } // Id użytkownika dodającego komentarz
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Post Post { get; set; }
    }

}
