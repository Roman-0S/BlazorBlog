using BlazorBlog.Client.Models;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {

        #region CreateCategories

        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);

        #endregion


        #region GetCategories

        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();

        Task<IEnumerable<CategoryDTO>> GetPopularCategoriesAsync(int count);

        Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId);

        #endregion


        #region UpdateCategories

        Task UpdateCategoryAsync(CategoryDTO category);

        #endregion


        #region DeleteCategories

        Task DeleteCategoryAsync(int categoryId);

        #endregion

    }
}
