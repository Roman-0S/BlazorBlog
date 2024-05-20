using BlazorBlog.Models;

namespace BlazorBlog.Services.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
    }
}
