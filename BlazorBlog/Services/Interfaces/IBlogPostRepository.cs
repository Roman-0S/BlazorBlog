using BlazorBlog.Models;

namespace BlazorBlog.Services.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetBlogPostsAsync();

        Task UpdateBlogPostAsync(BlogPost blogPost);

        Task<BlogPost?> GetBlogPostByIdAsync(int blogPostId);

        
    }
}
