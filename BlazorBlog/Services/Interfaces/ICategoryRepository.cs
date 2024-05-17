using BlazorBlog.Models;

namespace BlazorBlog.Services.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int categoryId);

        Task UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int categoryId);

    }
}
