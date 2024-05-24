using BlazorBlog.Data;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services
{
    public class CommentRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICommentRepository
    {
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Comment.Add(comment);
            await context.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteCommentByIdAsync(int id)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Comment? comment = await context.Comment.FirstOrDefaultAsync(c => c.Id == id);

            if (comment is not null)
            {
                context.Comment.Remove(comment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Comment? comment = await context.Comment.FirstOrDefaultAsync(c => c.Id == commentId);

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBlogPostIdAsync(int blogPostId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Comment> comments = await context.Comment.Where(c => c.BlogPostId == blogPostId).Include(c => c.AppUser).ToListAsync();

            return comments;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldUpdate = await context.Comment.AnyAsync(c => c.Id == comment.Id);

            if (shouldUpdate)
            {
                context.Comment.Update(comment);
                await context.SaveChangesAsync();
            }
        }
    }
}
