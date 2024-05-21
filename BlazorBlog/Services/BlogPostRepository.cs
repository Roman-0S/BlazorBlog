using BlazorBlog.Data;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorBlog.Services
{
    public class BlogPostRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IBlogPostRepository
    {
        public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            blogPost.Created = DateTimeOffset.Now;
            blogPost.Slug = await GenerateSlugAsync(blogPost.Title!, blogPost.Id);

            context.Add(blogPost);
            await context.SaveChangesAsync();

            return blogPost;
        }

        private async Task<string> GenerateSlugAsync(string title, int id)
        {

            if (await ValidateSlugAsync(title, id))
            {
                return Slugify(title);
            }
            else
            {
                // if the slug matches another slug, change the slug

                int i = 2;
                string newTitle = $"{title} {i}";
                bool isValid = await ValidateSlugAsync(newTitle, id);

                while (!isValid)
                {
                    i++;
                    newTitle = $"{title} {i}";
                    isValid = await ValidateSlugAsync(newTitle, id);
                }

                return Slugify(newTitle);
            }

        }

        private async Task<bool> ValidateSlugAsync(string title, int blogId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            string newSlug = Slugify(title);

            bool isValid;

            if (blogId == 0)
            {
                // this is a new post, so check if anything else has this slug

                isValid = !await context.BlogPosts.AnyAsync(bp => bp.Slug == newSlug);
            }
            else
            {
                // this is an existing post, see if any OTHER posts have this slug

                isValid = !await context.BlogPosts.AnyAsync(bp => bp.Slug == newSlug && bp.Id != blogId);
            }

            return isValid;
        }

        private static string Slugify(string title)
        {
            if (string.IsNullOrEmpty(title)) return title;

            title = title.Normalize(System.Text.NormalizationForm.FormD);

            char[] chars = title.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

            string normalizedTitle = new string(chars).Normalize(System.Text.NormalizationForm.FormC).ToLower().Trim();


            string titleWithoutSymbols = Regex.Replace(normalizedTitle, @"[^A-Za-z0-9\s-]", "");
            string slug = Regex.Replace(titleWithoutSymbols, @"\s+", "-");

            return slug;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPostsAsync()
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<BlogPost> blogPosts = await context.BlogPosts.Include(bp => bp.Category).ToListAsync();

            return blogPosts;
        }

        public async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldEdit = await context.BlogPosts.AnyAsync(c => c.Id == blogPost.Id);

            if (shouldEdit)
            {
                ImageUpload? oldImage = null;

                if (blogPost.Image is not null)
                {
                    context.Images.Add(blogPost.Image);

                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == blogPost.ImageId);

                    blogPost.ImageId = blogPost.ImageId;
                }

                blogPost.Slug = await GenerateSlugAsync(blogPost.Title!, blogPost!.Id);

                context.BlogPosts.Update(blogPost);
                await context.SaveChangesAsync();

                if (oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int blogPostId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.Include(bp => bp.Category).FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            return blogPost;
        }
    }
}
