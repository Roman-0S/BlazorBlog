using BlazorBlog.Client.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [Length(2, 50, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        public string? Name { get; set; }

        public virtual ICollection<BlogPost> Posts { get; set; } = new HashSet<BlogPost>();
    }

    public static class TagExtensions
    {
        public static TagDTO ToDTO(this Tag tag)
        {
            TagDTO dto = new TagDTO()
            {
                Id = tag.Id,
                Name = tag.Name,
            };

            foreach (BlogPost post in tag.Posts)
            {
                post.Tags.Clear();
                dto.Posts.Add(post.ToDTO());
            }

            return dto;
        }
    }
}
