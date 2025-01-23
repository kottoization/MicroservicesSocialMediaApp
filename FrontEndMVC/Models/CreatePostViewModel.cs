using System.ComponentModel.DataAnnotations;

namespace FrontEndMVC.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
    }
}