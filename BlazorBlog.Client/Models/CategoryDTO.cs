using BlazorBlog.Client.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Client.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50, ErrorMessage = "The {0} name should be between {2} and {1} characters long.")]
        public string? Name { get; set; }

        [MaxLength(200, ErrorMessage = "The description cannot exceed {1} characters.")]
        public string? Description { get; set; }

        public string ImageURL { get; set; } = ImageHelper.DefaultCategoryImage;

        public ICollection<BlogPostDTO> Posts { get; set; } = new HashSet<BlogPostDTO>();
    }
}
