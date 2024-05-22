using BlazorBlog.Models;

namespace BlazorBlog.Services.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetBlogPostsAsync();

        Task<BlogPost?> GetBlogPostBySlugAsync(string slug); 

        Task UpdateBlogPostAsync(BlogPost blogPost);

        Task<BlogPost?> GetBlogPostByIdAsync(int blogPostId);

        Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryIdAsync(int categoryId);

        Task<IEnumerable<BlogPost>> SearchBlogPostsAsync(string searchTerm);

        Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames);

        Task RemoveTagsFromBlogPostAsync(int blogPostId);
    }
}
