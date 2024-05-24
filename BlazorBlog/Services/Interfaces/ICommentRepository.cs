using BlazorBlog.Client;
using BlazorBlog.Models;

namespace BlazorBlog.Services.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment);

        Task<IEnumerable<Comment>> GetCommentsByBlogPostIdAsync(int blogPostId);

        Task<Comment?> GetCommentByIdAsync(int commentId);

        Task UpdateCommentAsync(Comment comment);

        Task DeleteCommentByIdAsync(int id);
    }
}
