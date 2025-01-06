namespace FrontEndMVC.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}