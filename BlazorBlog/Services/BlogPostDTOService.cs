using BlazorBlog.Client.Models;
using BlazorBlog.Client.Services.Interfaces;
using BlazorBlog.Helpers;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;

namespace BlazorBlog.Services
{
    public class BlogPostDTOService(IBlogPostRepository repository) : IBlogPostDTOService
    {
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

            // TODO: tags

            blogPost = await repository.CreateBlogPostAsync(blogPost);

            return blogPost.ToDTO();
        }

        public async Task<BlogPostDTO?> GetBlogPostByIdAsync(int blogPostId)
        {
            BlogPost? blogPost = await repository.GetBlogPostByIdAsync(blogPostId);

            return blogPost?.ToDTO();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsAsync()
        {
            IEnumerable<BlogPost> blogPosts = await repository.GetBlogPostsAsync();

            return blogPosts.Select(bp => bp.ToDTO());
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

        public async Task UpdateBlogPostAsync(BlogPostDTO blogPostDTO)
        {
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

                // TODO: Tags


                await repository.UpdateBlogPostAsync(blogPostToUpdate);
            }

        }
    }
}
