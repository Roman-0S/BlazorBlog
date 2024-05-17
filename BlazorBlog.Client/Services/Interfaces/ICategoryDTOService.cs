using BlazorBlog.Client.Models;

namespace BlazorBlog.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);

        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    }
}
