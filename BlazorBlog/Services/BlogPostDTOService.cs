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
    }
}
