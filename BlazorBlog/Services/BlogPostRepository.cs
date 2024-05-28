using BlazorBlog.Client.Models;
using BlazorBlog.Data;
using BlazorBlog.Helpers.Extensions;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorBlog.Services
{
    public class BlogPostRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IBlogPostRepository
    {

        #region CreateBlogPosts

        public async Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            blogPost.Created = DateTimeOffset.Now;
            blogPost.Slug = await GenerateSlugAsync(blogPost.Title!, blogPost.Id);

            context.Add(blogPost);
            await context.SaveChangesAsync();

            return blogPost;
        }

        #endregion


        #region GetBlogPosts

        public async Task<PagedList<BlogPost>> GetPublishedBlogPostsAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<BlogPost> blogPosts = await context.BlogPosts.Where(bp => bp.IsPublished && !bp.IsDeleted)
                                                                   .Include(bp => bp.Category)
                                                                   .Include(bp => bp.Comments)
                                                                   .OrderByDescending(bp => bp.Created)
                                                                   .ToPagedListAsync(page, pageSize);

            return blogPosts;
        }

        public async Task<PagedList<BlogPost>> GetDraftedBlogPostsAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<BlogPost> blogPosts = await context.BlogPosts.Where(bp => !bp.IsPublished && !bp.IsDeleted)
                                                                   .Include(bp => bp.Category)
                                                                   .Include(bp => bp.Comments)
                                                                   .OrderByDescending(bp => bp.Created)
                                                                   .ToPagedListAsync(page, pageSize);

            return blogPosts;
        }

        public async Task<PagedList<BlogPost>> GetDeletedBlogPostsAsync(int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<BlogPost> blogPosts = await context.BlogPosts.Where(bp => !bp.IsPublished && bp.IsDeleted)
                                                                   .Include(bp => bp.Category)
                                                                   .Include(bp => bp.Comments)
                                                                   .OrderByDescending(bp => bp.Created)
                                                                   .ToPagedListAsync(page, pageSize);

            return blogPosts;
        }

        public async Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync(int count)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<BlogPost> blogPosts = await context.BlogPosts.Where(bp => bp.IsPublished && !bp.IsDeleted)
                                                                     .Include(bp => bp.Comments)
                                                                     .OrderByDescending(bp => bp.Comments.Count)
                                                                     .Take(count).ToListAsync();

            return blogPosts;
        }


        #region GetBlogPostsBy

        public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.Where(bp => bp.IsPublished && !bp.IsDeleted)
                                                        .Include(bp => bp.Category)
                                                        .Include(bp => bp.Tags)
                                                        .Include(bp => bp.Comments)
                                                            .ThenInclude(c => c.AppUser)
                                                        .FirstOrDefaultAsync(bp => bp.Slug == slug);

            return blogPost;
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int blogPostId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.Include(bp => bp.Tags).FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPostsByCategoryIdAsync(int categoryId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            List<BlogPost> blogPosts = await context.BlogPosts.Include(bp => bp.Category).Where(bp => bp.Category!.Id == categoryId).ToListAsync();

            return blogPosts;
        }

        public async Task<PagedList<BlogPost>> GetPostsByCategoryId(int categoryId, int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<BlogPost> blogPosts = await context.BlogPosts.Where(bp => bp.IsPublished && !bp.IsDeleted && bp.CategoryId == categoryId)
                                                                   .Include(bp => bp.Category)
                                                                   .Include(bp => bp.Comments)
                                                                   .OrderByDescending(bp => bp.Created)
                                                                   .ToPagedListAsync(page, pageSize);

            return blogPosts;
        }

        public async Task<Tag?> GetTagByIdAsync(int tagId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);

            return tag;
        }

        public async Task<PagedList<BlogPost>> GetPostsByTagIdAsync(int tagId, int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<BlogPost> blogPosts = await context.BlogPosts.Where(bp => bp.IsPublished && !bp.IsDeleted)
                                                                   .Include(bp => bp.Category)
                                                                   .Include(bp => bp.Tags)
                                                                   .Include(bp => bp.Comments)
                                                                   .Where(bp => bp.Tags.Any(t => t.Id == tagId))
                                                                   .OrderByDescending(bp => bp.Created)
                                                                   .ToPagedListAsync(page, pageSize);

            return blogPosts;
        }

        public async Task<PagedList<BlogPost>> SearchPublishedBlogPostsAsync(string searchTerm, int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            string normalizedSearch = searchTerm.Trim().ToLower();

            PagedList<BlogPost> blogPosts = await context.BlogPosts.Include(bp => bp.Category)
                                                                   .Include(bp => bp.Tags)
                                                                   .Include(bp => bp.Comments)
                                                                       .ThenInclude(c => c.AppUser)
                                                                   .Where(bp => bp.IsPublished && !bp.IsDeleted)
                                                                   .Where(bp => string.IsNullOrEmpty(normalizedSearch)
                                                                             || bp.Title!.ToLower().Contains(normalizedSearch)
                                                                             || bp.Abstract!.ToLower().Contains(normalizedSearch)
                                                                             || bp.Content!.ToLower().Contains(normalizedSearch)
                                                                             || bp.Tags.Any(t => t.Name!.ToLower().Contains(normalizedSearch))
                                                                             || bp.Category!.Name!.ToLower().Contains(normalizedSearch)
                                                                             || bp.Comments.Any(c => c.Content!.ToLower().Contains(normalizedSearch)
                                                                                                  || c.AppUser!.FirstName!.ToLower().Contains(normalizedSearch)
                                                                                                  || c.AppUser!.LastName!.ToLower().Contains(normalizedSearch)))
                                                                   .OrderByDescending(bp => bp.Created)
                                                                   .ToPagedListAsync(page, pageSize);

            return blogPosts;
        }

        #endregion


        #endregion


        #region UpdateBlogPosts

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

        public async Task AddTagsToBlogPostAsync(int blogPostId, IEnumerable<string> tagNames)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            TextInfo textInfo = new CultureInfo("en-US").TextInfo;

            BlogPost? blogPost = await context.BlogPosts.Include(bp => bp.Tags).FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            if (blogPost is not null)
            {
                foreach(string tagName in tagNames)
                {
                    Tag? existingTag = await context.Tags.FirstOrDefaultAsync(t => t.Name!.Trim().ToLower() == tagName.Trim().ToLower());

                    if (existingTag is not null)
                    {
                        blogPost.Tags.Add(existingTag);
                    }
                    else
                    {
                        string titleCaseTagName = textInfo.ToTitleCase(tagName.Trim());

                        Tag newTag = new Tag() { Name = titleCaseTagName };

                        context.Tags.Add(newTag);
                        blogPost.Tags.Add(newTag);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagsFromBlogPostAsync(int blogPostId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            BlogPost? blogPost = await context.BlogPosts.Include(bp => bp.Tags).FirstOrDefaultAsync(bp => bp.Id == blogPostId);

            if (blogPost is not null)
            {
                blogPost.Tags.Clear();
                await context.SaveChangesAsync();
            }
        }

        #endregion


        #region Slug

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

        #endregion

    }
}
