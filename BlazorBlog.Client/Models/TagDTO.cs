using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Client.Models
{
    public class TagDTO
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        public string? Name { get; set; }

        public ICollection<BlogPostDTO> Posts { get; set; } = new HashSet<BlogPostDTO>();
    }
}
