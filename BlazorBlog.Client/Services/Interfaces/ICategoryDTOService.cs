using BlazorBlog.Client.Models;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);

        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();

        Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId);

        Task<IEnumerable<CategoryDTO>> GetPopularCategoriesAsync(int count);

        Task UpdateCategoryAsync(CategoryDTO category);

        Task DeleteCategoryAsync(int categoryId);
    }
}
