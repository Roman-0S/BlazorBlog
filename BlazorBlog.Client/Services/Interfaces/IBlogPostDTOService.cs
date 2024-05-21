using BlazorBlog.Client.Models;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);

        Task<IEnumerable<BlogPostDTO>> GetBlogPostsAsync();

        Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO);

        Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId);

    }
}
