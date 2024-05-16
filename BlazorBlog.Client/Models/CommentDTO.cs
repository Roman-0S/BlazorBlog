using BlazorBlog.Client.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Client.Models
{
    public class CommentDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset _updated;

        public int Id { get; set; }

        [Required]
        [Length(2, 5000, ErrorMessage = "Your {0} must be between {2} and {1} characters long.")]
        public string? Content { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created;

            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset Updated
        {
            get => _updated;

            set => _updated = value.ToUniversalTime();
        }

        [Display(Name = "Update Description")]
        [MaxLength(200, ErrorMessage = "Your update description must not exceed {1} characters.")]
        public string? UpdateDescription { get; set; }

        public string? AuthorId { get; set; }

        public string? AuthorName { get; set; }

        public string AuthorImage { get; set; } = ImageHelper.DefaultProfilePicture;

        public int BlogPostId { get; set; }
    }
}
