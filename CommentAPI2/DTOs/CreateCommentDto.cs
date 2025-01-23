namespace CommentAPI2.DTOs
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid PostId { get; set; }
    }
}
