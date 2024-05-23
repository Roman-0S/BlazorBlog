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

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await repository.DeleteCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await repository.GetCategoriesAsync();

            return categories.Select(c => c.ToDTO());
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId)
        {
            Category? category = await repository.GetCategoryByIdAsync(categoryId);

            return category?.ToDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetPopularCategoriesAsync(int count)
        {
            IEnumerable<Category> categories = await repository.GetPopularCategoriesAsync(count);

            return categories.Select(c => c.ToDTO());
        }

        public async Task UpdateCategoryAsync(CategoryDTO category)
        {
            Category? categoryToUpdate = await repository.GetCategoryByIdAsync(category.Id);

            if (categoryToUpdate is not null)
            {
                categoryToUpdate.Name = category.Name;

                categoryToUpdate.Description = category.Description;

                if (category.ImageURL.StartsWith("data:"))
                {
                    categoryToUpdate.Image = UploadHelper.GetImageUpload(category.ImageURL);
                }

                await repository.UpdateCategoryAsync(categoryToUpdate);
            }
        }
    }
}
