using BlazorBlog.Client.Models;
using BlazorBlog.Client.Services.Interfaces;
using BlazorBlog.Helpers;
using BlazorBlog.Models;
using BlazorBlog.Services.Interfaces;

namespace BlazorBlog.Services
{
    public class CategoryDTOService(ICategoryRepository repository) : ICategoryDTOService
    {
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description,
            };

            if (category.ImageURL.StartsWith("data:"))
            {
                newCategory.Image = UploadHelper.GetImageUpload(category.ImageURL);
            }

            Category createdCategory = await repository.CreateCategoryAsync(newCategory);

            return createdCategory.ToDTO();

        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await repository.GetCategoriesAsync();

            return categories.Select(c => c.ToDTO());
        }
    }
}
