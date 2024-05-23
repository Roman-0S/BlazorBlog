using BlazorBlog.Client.Models;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface IBlogPostDTOService
    {

        #region CreateBlogPosts

        Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO);

        #endregion


        #region GetBlogPosts

        Task<IEnumerable<BlogPostDTO>> GetPublishedBlogPostsAsync();

        Task<IEnumerable<BlogPostDTO>> GetDraftedBlogPostsAsync();

        Task<IEnumerable<BlogPostDTO>> GetDeletedBlogPostsAsync();

        Task<IEnumerable<BlogPostDTO>> GetPopularBlogPostsAsync(int count);


        #region GetBlogPostsBy

        Task<BlogPostDTO?> GetBlogPostBySlugAsync(string slug);

        Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId);

        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryIdAsync(int categoryId);

        Task<IEnumerable<BlogPostDTO>> SearchBlogPostsAsync(string searchTerm);

        #endregion


        #endregion


        #region UpdateBlogPosts

        Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO);

        #endregion

    }
}
