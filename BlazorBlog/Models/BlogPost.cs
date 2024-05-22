using BlazorBlog.Client.Models;
using BlazorBlog.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Models
{
    public class BlogPost
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

        public DateTimeOffset? Updated
        {
            get => _updated;

            set => _updated = value?.ToUniversalTime();
        }


        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? ImageId { get; set; }

        public virtual ImageUpload? Image { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    }

    public static class BlogPostExtensions
    {
        public static BlogPostDTO ToDTO(this BlogPost post)
        {
            BlogPostDTO dto = new BlogPostDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                Abstract = post.Abstract,
                Content = post.Content,
                Created = post.Created,
                Updated = post.Updated,
                IsPublished = post.IsPublished,
                IsDeleted = post.IsDeleted,
                CategoryId = post.CategoryId,
                ImageURL = post.ImageId.HasValue ? $"api/uploads/{post.ImageId}" : UploadHelper.DefaultBlogImage
            };

            if (post.Category is not null)
            {
                post.Category.Posts = [];

                dto.Category = post.Category.ToDTO();
            }

            foreach (Comment comment in post.Comments)
            {
                dto.Comments.Add(comment.ToDTO());
            }

            foreach (Tag tag in post.Tags)
            {
                tag.Posts.Clear();
                dto.Tags.Add(tag.ToDTO());
            }


            return dto;
        }
    }
}
