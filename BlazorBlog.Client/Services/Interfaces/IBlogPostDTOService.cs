using BlazorBlog.Client.Models;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface IBlogPostDTOService
    {

        #region CreateBlogPosts

        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);

        #endregion


        #region GetBlogPosts

        Task<PagedList<BlogPostDTO>> GetPublishedBlogPostsAsync(int page, int pageSize);

        Task<PagedList<BlogPostDTO>> GetDraftedBlogPostsAsync(int page, int pageSize);

        Task<PagedList<BlogPostDTO>> GetDeletedBlogPostsAsync(int page, int pageSize);

        Task<IEnumerable<BlogPostDTO>> GetPopularBlogPostsAsync(int count);


        #region GetBlogPostsBy

        Task<BlogPostDTO?> GetBlogPostBySlugAsync(string slug);

        Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId);

        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryIdAsync(int categoryId);

        Task<PagedList<BlogPostDTO>> SearchPublishedBlogPostsAsync(string searchTerm, int page, int pageSize);

        #endregion


        #endregion


        #region UpdateBlogPosts

        Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO);

        #endregion

    }
}
