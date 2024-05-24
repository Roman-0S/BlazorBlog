using BlazorBlog.Client.Models;
using System.Xml.Linq;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface ICommentDTOService
    {
        Task<CommentDTO> CreateCommentAsync(CommentDTO commentDTO);

        Task<IEnumerable<CommentDTO>> GetCommentsByBlogPostIdAsync(int blogPostId);

        Task<CommentDTO?> GetCommentByIdAsync(int commentId);

        Task UpdateCommentAsync(CommentDTO comment);

        Task DeleteCommentByIdAsync(int id);

    }
}
