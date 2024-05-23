using BlazorBlog.Client.Models;
using BlazorBlog.Client.Services.Interfaces;
using BlazorBlog.Helpers;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;

namespace BlazorBlog.Services
{
    public class BlogPostDTOService(IBlogPostRepository repository) : IBlogPostDTOService
    {

        #region CreateBlogPosts

        public async Task<BlogPostDTO> CreateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
            BlogPost blogPost = new BlogPost()
            {
                Title = blogPostDTO.Title,
                Abstract = blogPostDTO.Abstract,
                Content = blogPostDTO.Content,
                IsPublished = blogPostDTO.IsPublished,
                CategoryId = blogPostDTO.CategoryId,
                Created = DateTimeOffset.Now,
            };

            if (blogPostDTO.ImageURL?.StartsWith("data:") == true)
            {
                try
                {
                    blogPost.Image = UploadHelper.GetImageUpload(blogPostDTO.ImageURL);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

            blogPost = await repository.CreateBlogPostAsync(blogPost);

            IEnumerable<string> tagNames = blogPostDTO.Tags.Select(t => t.Name!);
            await repository.AddTagsToBlogPostAsync(blogPost.Id, tagNames);

            return blogPost.ToDTO();
        }

        #endregion


        #region GetBlogPosts

        public async Task<IEnumerable<BlogPostDTO>> GetPublishedBlogPostsAsync()
        {
            IEnumerable<BlogPost> blogPosts = await repository.GetPublishedBlogPostsAsync();

            return blogPosts.Select(bp => bp.ToDTO());
        }

        public async Task<IEnumerable<BlogPostDTO>> GetDraftedBlogPostsAsync()
        {
            IEnumerable<BlogPost> blogPosts = await repository.GetDraftedBlogPostsAsync();

            return blogPosts.Select(bp => bp.ToDTO());
        }

        public async Task<IEnumerable<BlogPostDTO>> GetDeletedBlogPostsAsync()
        {
            IEnumerable<BlogPost> blogPosts = await repository.GetDeletedBlogPostsAsync();

            return blogPosts.Select(bp => bp.ToDTO());
        }

        public async Task<IEnumerable<BlogPostDTO>> GetPopularBlogPostsAsync(int count)
        {
            IEnumerable<BlogPost> blogPosts = await repository.GetPopularBlogPostsAsync(count);

            return blogPosts.Select(bp => bp.ToDTO());
        }


        #region GetBlogPostsBy

        public async Task<BlogPostDTO?> GetBlogPostBySlugAsync(string slug)
        {
            BlogPost? blogPost = await repository.GetBlogPostBySlugAsync(slug);

            return blogPost?.ToDTO();
        }

        public async Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId)
        {
            BlogPost? blogPost = await repository.GetBlogPostByIdAsync(blogPostId);

            return blogPost?.ToDTO();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryIdAsync(int categoryId)
        {
            IEnumerable<BlogPost> blogPosts = await repository.GetBlogPostsByCategoryIdAsync(categoryId);

            return blogPosts.Select(bp => bp.ToDTO());
        }

        public async Task<IEnumerable<BlogPostDTO>> SearchBlogPostsAsync(string searchTerm)
        {
            IEnumerable<BlogPost> blogPosts = await repository.SearchBlogPostsAsync(searchTerm);

            return blogPosts.Select(bp => bp.ToDTO());
        }

        #endregion


        #endregion


        #region UpdateBlogPosts

        public async Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
            await repository.RemoveTagsFromBlogPostAsync(blogPostDTO.Id);

            BlogPost? blogPostToUpdate = await repository.GetBlogPostByIdAsync(blogPostDTO.Id);

            if (blogPostToUpdate is not null)
            {
                blogPostToUpdate.Title = blogPostDTO.Title;

                blogPostToUpdate.Abstract = blogPostDTO.Abstract;

                blogPostToUpdate.Content = blogPostDTO.Content;

                blogPostToUpdate.IsPublished = blogPostDTO.IsPublished;

                blogPostToUpdate.IsDeleted = blogPostDTO.IsDeleted;

                blogPostToUpdate.CategoryId = blogPostDTO.CategoryId;

                blogPostToUpdate.Updated = DateTimeOffset.Now;


                if (blogPostDTO.ImageURL.StartsWith("data:"))
                {
                    blogPostToUpdate.Image = UploadHelper.GetImageUpload(blogPostDTO.ImageURL);
                }

                await repository.UpdateBlogPostAsync(blogPostToUpdate);


                IEnumerable<string> tagNames = blogPostDTO.Tags.Select(t => t.Name!);

                await repository.AddTagsToBlogPostAsync(blogPostToUpdate.Id, tagNames);
            }

        }

        #endregion

    }
}
