using BlazorBlog.Client.Models;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface IBlogPostDTOService
    {
        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);

        Task<IEnumerable<BlogPostDTO>> GetBlogPostsAsync();

        Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO);

        Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId);

        Task<BlogPostDTO?> GetBlogPostBySlugAsync(string slug);

        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryIdAsync(int categoryId);

        Task<IEnumerable<BlogPostDTO>> SearchBlogPostsAsync(string searchTerm);
    }
}
