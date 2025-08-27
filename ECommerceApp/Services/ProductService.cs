using Microsoft.EntityFrameworkCore;
using ECommerceApp.DTOs.ProductDTOs;
using ECommerceApp.Models;
using ECommerceApp.DTOs;
using ECommerceApp.Data;

namespace ECommerceApp.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductCreateDTO productcreateDto)
        {
            try
            {
                if (await _context.Products.AnyAsync(p => p.Name.ToLower().Replace(" ", "") == productcreateDto.Name.ToLower().Replace(" ", "")))
                {
                    return new ApiResponse<ProductResponseDTO>(400, "Product with the same name already exists.");
                }
                if (!await _context.Categories.AnyAsync(c => c.Id == productcreateDto.CategoryId))
                {
                    return new ApiResponse<ProductResponseDTO>(400, "Invalid Category ID. Category does not exist.");
                }
                var product = new Product
                {
                    Name = productcreateDto.Name,
                    Description = productcreateDto.Description,
                    Price = productcreateDto.Price,
                    StockQuanity = productcreateDto.StockQuantity,
                    ImageUrl = productcreateDto.ImageUrl,
                    DiscoutPercentage = productcreateDto.DiscountPercentage,
                    CategoryId = productcreateDto.CategoryId,
                    IsAvailable = productcreateDto.StockQuantity > 0
                };
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                var productResponseDto = new ProductResponseDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuanity,
                    ImageUrl = product.ImageUrl,
                    DiscountPercentage = product.DiscoutPercentage,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable
                };
                return new ApiResponse<ProductResponseDTO>(200, productResponseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponseDTO>(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ProductResponseDTO>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new ApiResponse<ProductResponseDTO>(404, "Product not found.");
                }
                var productResponseDto = new ProductResponseDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuanity,
                    ImageUrl = product.ImageUrl,
                    DiscountPercentage = product.DiscoutPercentage,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable
                };
                return new ApiResponse<ProductResponseDTO>(200, productResponseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponseDTO>(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductAsync(ProductUpdateDTO productUpdateDto)
        {
            try
            {
                var product = await _context.Products.FindAsync(productUpdateDto.Id);
                if (product == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }
                if (await _context.Products.AnyAsync(p => p.Id != productUpdateDto.Id && p.Name.ToLower().Replace(" ", "") == productUpdateDto.Name.ToLower().Replace(" ", "")))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Another product with the same name already exists.");
                }
                if (!await _context.Categories.AnyAsync(c => c.Id == productUpdateDto.CategoryId))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Specified Category does not exist.");
                }
                product.Name = productUpdateDto.Name;
                product.Description = productUpdateDto.Description;
                product.Price = productUpdateDto.Price;
                product.StockQuanity = productUpdateDto.StockQuantity;
                product.ImageUrl = productUpdateDto.ImageUrl;
                product.DiscoutPercentage = productUpdateDto.DiscountPercentage;
                product.CategoryId = productUpdateDto.CategoryId;
                await _context.SaveChangesAsync();

                var confirmationResponse = new ConfirmationResponseDTO
                {
                    Message = $"Product with ID {product.Id} has been successfully updated."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }
                product.IsAvailable = false;
                await _context.SaveChangesAsync();
                var confirmationResponse = new ConfirmationResponseDTO
                {
                    Message = $"Product with ID {id} has been successfully deleted."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();
                var productResponseDtos = products.Select(product => new ProductResponseDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuanity,
                    ImageUrl = product.ImageUrl,
                    DiscountPercentage = product.DiscoutPercentage,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable
                }).ToList();
                return new ApiResponse<List<ProductResponseDTO>>(200, productResponseDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductResponseDTO>>(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _context.Products.AsNoTracking().Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId && p.IsAvailable).ToListAsync();

                if(products == null || products.Count == 0)
                {
                    return new ApiResponse<List<ProductResponseDTO>>(404, "No products found for the specified category.");
                }
                var productResponseDtos = products.Select(product => new ProductResponseDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuanity,
                    ImageUrl = product.ImageUrl,
                    DiscountPercentage = product.DiscoutPercentage,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable
                }).ToList();
                return new ApiResponse<List<ProductResponseDTO>>(200, productResponseDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductResponseDTO>>(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductStates(ProductStatusUpdateDTO productStatusUpdate)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == productStatusUpdate.ProductId);
                if(product == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found.");
                }
                product.IsAvailable = productStatusUpdate.IsAvailable;
                await _context.SaveChangesAsync();

                var confirmationResponse = new ConfirmationResponseDTO
                {
                    Message = $"Product with ID {product.Id} availability status has been updated to {(product.IsAvailable ? "Available" : "Unavailable")}."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);

            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
