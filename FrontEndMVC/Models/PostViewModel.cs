namespace FrontEndMVC.Models
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
