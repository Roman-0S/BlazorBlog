using BlazorBlog.Client.Models;
using BlazorBlog.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50, ErrorMessage = "The {0} name should be between {2} and {1} characters long.")]
        public string? Name { get; set; }

        [MaxLength(200, ErrorMessage = "The description cannot exceed {1} characters.")]
        public string? Description { get; set; }

        public Guid? ImageId { get; set; }

        public virtual ImageUpload? Image { get; set; }

        public virtual ICollection<BlogPost> Posts { get; set; } = new HashSet<BlogPost>();
    }

    public static class CategoryExtensions
    {
        public static CategoryDTO ToDTO(this Category category)
        {
            CategoryDTO dto = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageURL = category.ImageId.HasValue ? $"api/uploads/{category.ImageId}" : UploadHelper.DefaultCategoryImage
            };

            foreach (BlogPost post in category.Posts)
            {
                category.Posts.Clear();
                dto.Posts.Add(post.ToDTO());
            }

            return dto;
        }
    }
}
