namespace PostAPI.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // Id u¿ytkownika tworz¹cego post
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }

}
