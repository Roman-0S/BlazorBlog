using BlazorBlog.Client.Helpers;
using BlazorBlog.Client.Models;
using BlazorBlog.Data;
using BlazorBlog.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Models
{
    public class Comment
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

        [Display (Name = "Update Description")]
        [MaxLength(200, ErrorMessage = "Your update description must not exceed {1} characters.")]
        public string? UpdateDescription { get; set; }

        [Required]
        public string? AppUserId { get; set; }

        public virtual ApplicationUser? AppUser { get; set; }

        public int BlogPostId { get; set; }

        public virtual BlogPost? BlogPost { get; set; }
    }

    public static class CommentExtensions
    {
        public static CommentDTO ToDTO(this Comment comment)
        {
            CommentDTO dto = new CommentDTO()
            {
                Id = comment.Id,
                Content = comment.Content,
                Created = comment.Created,
                Updated = comment.Updated,
                UpdateDescription = comment.UpdateDescription,
                BlogPostId = comment.BlogPostId,
                AuthorId = comment.AppUserId,
                AuthorName = comment.AppUser?.FullName,
                AuthorImage = comment.AppUser?.ImageId.HasValue == true ? $"api/uploads/{comment.AppUser.ImageId}" : UploadHelper.DefaultProfilePicture,
            };

            return dto;
        }
    }
}
