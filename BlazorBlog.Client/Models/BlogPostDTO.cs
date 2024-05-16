using System.ComponentModel.DataAnnotations;
using BlazorBlog.Client.Helpers;
using System.Xml.Linq;

namespace BlazorBlog.Client.Models
{
    public class BlogPostDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;

        public int Id { get; set; }

        [Required]
        [Length(2, 200, ErrorMessage = "The {0} should be between {2} and {1} characters long.")]
        public string? Title { get; set; }

        [Required]
        public string? Slug { get; set; }

        [Required]
        [Length(2, 600, ErrorMessage = "The {0} should be between {2} and {1} characters long.")]
        public string? Abstract { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created;

            set => _created = value.ToUniversalTime();
        }

        [Required]
        public DateTimeOffset? Updated
        {
            get => _updated;

            set => _updated = value?.ToUniversalTime();
        }


        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }

        public string ImageURL { get; set; } = ImageHelper.DefaultBlogImage;

        public int CategoryId { get; set; }

        public CategoryDTO? Category { get; set; }
        public ICollection<CommentDTO> Comments { get; set; } = new HashSet<CommentDTO>();

        public ICollection<TagDTO> Tags { get; set; } = new HashSet<TagDTO>();
    }
}
