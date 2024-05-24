using BlazorBlog.Client.Models;
using BlazorBlog.Client.Services.Interfaces;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;

namespace BlazorBlog.Services
{
    public class CommentDTOService(ICommentRepository repository) : ICommentDTOService
    {
        public async Task<CommentDTO> CreateCommentAsync(CommentDTO commentDTO)
        {
            Comment comment = new()
            {
                Content = commentDTO.Content,
                Created = DateTimeOffset.Now,
                AppUserId = commentDTO.AuthorId,
                BlogPostId = commentDTO.BlogPostId,
            };

            comment = await repository.CreateCommentAsync(comment);

            return comment.ToDTO();
        }

        public async Task DeleteCommentByIdAsync(int id)
        {
            await repository.DeleteCommentByIdAsync(id);
        }

        public async Task<CommentDTO?> GetCommentByIdAsync(int commentId)
        {
            Comment? comment = await repository.GetCommentByIdAsync(commentId);

            return comment?.ToDTO();
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId)
        {
            IEnumerable<Comment> comments = await repository.GetCommentsByBlogPostIdAsync(blogPostId);

            List<CommentDTO> dtos = new();

            foreach (Comment comment in comments)
            {
                CommentDTO commentDTO = comment.ToDTO();

                commentDTO.AuthorId = comment.AppUserId;

                dtos.Add(commentDTO);
            }

            return dtos;
        }

        public async Task UpdateCommentAsync(CommentDTO comment)
        {
            Comment? commentToUpdate = await repository.GetCommentByIdAsync(comment.Id);

            if (commentToUpdate is not null)
            {
                commentToUpdate.Updated = DateTimeOffset.Now;
                commentToUpdate.Content = comment.Content;
                commentToUpdate.UpdateDescription = comment.UpdateDescription;

                await repository.UpdateCommentAsync(commentToUpdate);
            }
        }
    }
}
