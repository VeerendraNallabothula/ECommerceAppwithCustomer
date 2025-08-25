using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Services;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CategoryDTOs;
namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ApiResponse<CategoryCreateDTO>>> CreateCategory([FromBody] CategoryCreateDTO createDto)
        {
            var response = await _categoryService.CreateCategoryAsync(createDto);
            if(response.StatusCode != 200)
            {
                return (StatusCode(response.StatusCode, response));
            }
            return Ok(response);
        }
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCategory([FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            var response = await _categoryService.UpdateCategoryAsync(categoryUpdateDTO);
            if (response.StatusCode != 200)
            {
                return (StatusCode(response.StatusCode, response));
            }
            return Ok(response);
        }
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCategory(int Id)
        {
            var response = await _categoryService.DeleteCategoryAsync(Id);
            if (response.StatusCode != 200)
            {
                return (StatusCode(response.StatusCode, response));
            }
            return Ok(response);
        }
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<ApiResponse<List<CategoryResponseDTO>>>> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            if (response.StatusCode != 200)
            {
                return (StatusCode(response.StatusCode, response));
            }
            return Ok(response);
        }
    }
}
