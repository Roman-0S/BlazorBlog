﻿using BlazorBlog.Client.Models;
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

        public async Task<PagedList<BlogPostDTO>> GetPublishedBlogPostsAsync(int page, int pageSize)
        {
            PagedList<BlogPost> blogPosts = await repository.GetPublishedBlogPostsAsync(page, pageSize);

            PagedList<BlogPostDTO> dtos = new PagedList<BlogPostDTO>()
            {
                Page = blogPosts.Page,
                TotalPages = blogPosts.TotalPages,
                TotalItems = blogPosts.TotalItems,
                Data = blogPosts.Data.Select(bp => bp.ToDTO()),
            };

            return dtos;
        }

        public async Task<PagedList<BlogPostDTO>> GetDraftedBlogPostsAsync(int page, int pageSize)
        {
            PagedList<BlogPost> blogPosts = await repository.GetDraftedBlogPostsAsync(page, pageSize);

            PagedList<BlogPostDTO> dtos = new()
            {
                Page = blogPosts.Page,
                TotalPages = blogPosts.TotalPages,
                TotalItems = blogPosts.TotalItems,
                Data = blogPosts.Data.Select(bp => bp.ToDTO()),
            };

            return dtos;
        }

        public async Task<PagedList<BlogPostDTO>> GetDeletedBlogPostsAsync(int page, int pageSize)
        {
            PagedList<BlogPost> blogPosts = await repository.GetDeletedBlogPostsAsync(page, pageSize);

            PagedList<BlogPostDTO> dtos = new()
            {
                Page = blogPosts.Page,
                TotalPages = blogPosts.TotalPages,
                TotalItems = blogPosts.TotalItems,
                Data = blogPosts.Data.Select(bp => bp.ToDTO()),
            };

            return dtos;
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

        public async Task<PagedList<BlogPostDTO>> GetPostsByCategoryId(int categoryId, int page, int pageSize)
        {
            PagedList<BlogPost> blogPosts = await repository.GetPostsByCategoryId(categoryId, page, pageSize);

            PagedList<BlogPostDTO> dtos = new PagedList<BlogPostDTO>()
            {
                Page = blogPosts.Page,
                TotalPages = blogPosts.TotalPages,
                TotalItems = blogPosts.TotalItems,
                Data = blogPosts.Data.Select(bp => bp.ToDTO()),
            };

            return dtos;
        }

        public async Task<TagDTO?> GetTagByIdAsync(int tagId)
        {
            Tag? tag = await repository.GetTagByIdAsync(tagId);

            return tag?.ToDTO();
        }

        public async Task<PagedList<BlogPostDTO>> GetPostsByTagIdAsync(int tagId, int page, int pageSize)
        {
            PagedList<BlogPost> blogPosts = await repository.GetPostsByTagIdAsync(tagId, page, pageSize);

            PagedList<BlogPostDTO> dtos = new()
            {
                Page = blogPosts.Page,
                TotalPages = blogPosts.TotalPages,
                TotalItems = blogPosts.TotalItems,
                Data = blogPosts.Data.Select(bp => bp.ToDTO()),
            };

            return dtos;
        }

        public async Task<PagedList<BlogPostDTO>> SearchPublishedBlogPostsAsync(string searchTerm, int page, int pageSize)
        {
            PagedList<BlogPost> blogPosts = await repository.SearchPublishedBlogPostsAsync(searchTerm, page, pageSize);

            PagedList<BlogPostDTO> dtos = new()
            {
                Page = blogPosts.Page,
                TotalPages = blogPosts.TotalPages,
                TotalItems = blogPosts.TotalItems,
                Data = blogPosts.Data.Select(bp => bp.ToDTO()),
            };

            return dtos;
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
