using BlazorBlog.Client.Models;
using BlazorBlog.Models;

namespace BlazorBlog.Services.Interfaces
{
    public interface IBlogPostRepository
    {

        #region CreateBlogPosts

        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);

        #endregion


        #region GetBlogPosts

        Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize);

        Task<IEnumerable<BlogPost>> GetDraftedBlogPostsAsync();

        Task<IEnumerable<BlogPost>> GetDeletedBlogPostsAsync();

        Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync(int count);


        #region GetBlogPostsBy

        Task<BlogPost?> GetBlogPostBySlugAsync(string slug);

        Task<BlogPost?> GetBlogPostByIdAsync(int blogPostId);

        Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryIdAsync(int categoryId);

        Task<IEnumerable<BlogPost>> SearchBlogPostsAsync(string searchTerm);

        #endregion


        #endregion


        #region UpdateBlogPosts

        Task UpdateBlogPostAsync(BlogPost blogPost);

        Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames);

        Task RemoveTagsFromBlogPostAsync(int blogPostId);

        #endregion

    }
}
