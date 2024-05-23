using BlazorBlog.Models;

namespace BlazorBlog.Services.Interfaces
{
    public interface ICategoryRepository
    {

        #region CreateCategories

        Task<Category> CreateCategoryAsync(Category category);

        #endregion


        #region GetCategories

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<IEnumerable<Category>> GetPopularCategoriesAsync(int count);

        Task<Category?> GetCategoryByIdAsync(int categoryId);

        #endregion


        #region UpdateCategories

        Task UpdateCategoryAsync(Category category);

        #endregion


        #region DeleteCategories

        Task DeleteCategoryAsync(int categoryId);

        #endregion

    }
}
