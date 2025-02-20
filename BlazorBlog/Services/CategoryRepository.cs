﻿using BlazorBlog.Data;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICategoryRepository
    {

        #region CreateCategories

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return category;
        }

        #endregion


        #region GetCategories

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Category> categories = await context.Categories.Include(c => c.Posts).ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<Category>> GetPopularCategoriesAsync(int count)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Category> categories = await context.Categories.Include(c => c.Posts.Where(p => p.IsPublished && !p.IsDeleted))
                                                                       .OrderByDescending(c => c.Posts.Where(p => p.IsPublished && !p.IsDeleted).Count())
                                                                       .Take(count)
                                                                       .ToListAsync();

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            return category;
        }

        #endregion


        #region UpdateCategories

        public async Task UpdateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldUpdate = await context.Categories.AnyAsync(c => c.Id == category.Id);

            if (shouldUpdate)
            {
                context.Categories.Update(category);
                await context.SaveChangesAsync();
            }
        }

        #endregion


        #region DeleteCategories

        public async Task DeleteCategoryAsync(int categoryId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category is not null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }

        #endregion

    }
}
