using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.DTOs.CategoryDTOs;
using ECommerceApp.Models;
using ECommerceApp.DTOs;
namespace ECommerceApp.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryCreateDTO createcategoryDto)
        {
            try
            {
                if (await _context.Categories.AnyAsync(c => c.Name.ToLower().Replace(" ", "") == createcategoryDto.Name.ToLower().Replace(" ", "")))
                {
                    return new ApiResponse<CategoryResponseDTO>(400, "Category with the same name already exists.");
                }
                var category = new Category
                {
                    Name = createcategoryDto.Name,
                    Description = createcategoryDto.Description,
                    IsActive = true
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var categoryResponseDto = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive
                };
                return new ApiResponse<CategoryResponseDTO>(200, "Category created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"An error occurred while creating the category: {ex.Message}");
            }
        }
        public async Task<ApiResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return new ApiResponse<CategoryResponseDTO>(404, "Category not found.");
                }
                var categoryResponseDto = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive
                };
                return new ApiResponse<CategoryResponseDTO>(200, "Category retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"An error occurred while retrieving the category: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO updatecategoryDto)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updatecategoryDto.Id);
                if (category == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");
                }
                if (await _context.Categories.AnyAsync(c => c.Id != updatecategoryDto.Id && c.Name.ToLower().Replace(" ", "") == updatecategoryDto.Name.ToLower().Replace(" ", "")))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Another category with the same name already exists.");
                }
                category.Name = updatecategoryDto.Name;
                category.Description = updatecategoryDto.Description;
                await _context.SaveChangesAsync();
                var confirmationResponseDto = new ConfirmationResponseDTO
                {
                    Message = $"Category with Id {updatecategoryDto.Id} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An error occurred while updating the category: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");
                }
                if (category == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Category not found");
                }
                category.IsActive = false;
                await _context.SaveChangesAsync();

                var confirmationResponseDto = new ConfirmationResponseDTO
                {
                    Message = $"Category with Id {id} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An error occurred while deactivating the category: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories.AsNoTracking().ToListAsync();
                var categoryList = categories.Select(c => new CategoryResponseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive
                }).ToList();
                return new ApiResponse<List<CategoryResponseDTO>>(200, categoryList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoryResponseDTO>>(500, $"An error occurred while retrieving categories: {ex.Message}");
            }
        }
    }
}
