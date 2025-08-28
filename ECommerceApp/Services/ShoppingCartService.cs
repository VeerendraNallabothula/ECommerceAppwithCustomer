using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.DTOs.ShoppingCartDTOs;
using ECommerceApp.DTOs;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
namespace ECommerceApp.Services
{
    public class ShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CartResponseDTO>> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsCheckedOut);
                if (cart == null)
                {
                    var emptyCart = new CartResponseDTO
                    {
                        CustomerId = customerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TotalBasePrice = 0,
                        TotalDiscount = 0,
                        TotalAmount = 0,
                        CartItems = new List<CartItemResponseDTO>()
                    };
                    return new ApiResponse<CartResponseDTO>(200, emptyCart);
                }
                var cartDTO = MapCartToDto(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);

            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An error occurred while" +
                    $"while processing your request: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CartResponseDTO>> AddtoCartAsync(AddToCartDTO addToCartDTO)
        {
            try
            {
                var product = await _context.Products.FindAsync(addToCartDTO.ProductId);
                if (product == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Product not found.");
                }

                if (addToCartDTO.Quantity > product.StockQuanity)
                {
                    return new ApiResponse<CartResponseDTO>(400, $"Only  " +
                        $"{product.StockQuanity} units of {product.Name} are Avialable.");
                }
                var cart =  await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.CustomerId == addToCartDTO.CustomerId && !c.IsCheckedOut);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        CustomerId = addToCartDTO.CustomerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                var existingCartItem = cart.CartItems.
                    FirstOrDefault(ci => ci.ProductId == addToCartDTO.ProductId);

                    if (existingCartItem != null)
                    {
                        if(existingCartItem.Quantity + addToCartDTO.Quantity > product.StockQuanity)
                        {
                            return new ApiResponse<CartResponseDTO>
                                (400, $"Adding {addToCartDTO.Quantity} exceeds available stock.");
                        }
                        existingCartItem.Quantity += addToCartDTO.Quantity;
                        existingCartItem.TotalPrice = (existingCartItem.UnitPrice - existingCartItem.Discount) * 
                            existingCartItem.Quantity;
                        existingCartItem.UpdatedAt = DateTime.UtcNow;
                        _context.CartItems.Update(existingCartItem);

                    }
                    else
                    {
                        var discount = product.DiscoutPercentage > 0 ? product.Price
                            * product.DiscoutPercentage / 100 : 0;

                        var cartItem = new CartItem
                        {
                            CartId = cart.Id,
                            ProductId = product.Id,
                            Quantity = addToCartDTO.Quantity,
                            UnitPrice = product.Price,
                            Discount = discount,
                            TotalPrice = (product.Price - discount) * addToCartDTO.Quantity,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow

                        };
                        _context.CartItems.Add(cartItem);
                    }
                    cart.UpdatedAt = DateTime.UtcNow;
                    _context.Carts.Update(cart);
                    await _context.SaveChangesAsync();

                    cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                        .FirstOrDefaultAsync(c => c.Id == cart.Id) ?? new Cart();

                    var cartDTO = MapCartToDto(cart);
                    return new ApiResponse<CartResponseDTO>(200, cartDTO);

             }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An error occurred while adding to cart: {ex.Message}");
            }

        }
        public async Task<ApiResponse<CartResponseDTO>> UpdateCartItemAsync(UpdateCartItemDTO updateCartItemDTO)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(c => c.Product)
                    .FirstOrDefaultAsync(c => c.CustomerId == updateCartItemDTO.CustomerId && !c.IsCheckedOut);

                if(cart == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "ActiveCart is not found");
                }

                var cartItem = cart.CartItems.FirstOrDefault(C => C.Id == updateCartItemDTO.CartItemId);

                if(cartItem == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Cart item is not found");
                }

                if(updateCartItemDTO.Quantity > cartItem.Product.StockQuanity)
                {
                    return new ApiResponse<CartResponseDTO>(400, $"only {cartItem.Product.StockQuanity}" +
                        $"units of {cartItem.Product.Name} are available.");
                }
                cartItem.Quantity = updateCartItemDTO.Quantity;
                cartItem.TotalPrice = (cartItem.UnitPrice - cartItem.Discount) * cartItem.Quantity;
                cartItem.UpdatedAt = DateTime.UtcNow;
                _context.CartItems.Update(cartItem);

                cart.UpdatedAt = DateTime.UtcNow;
                _context.Carts.Update(cart);

                await _context.SaveChangesAsync();  

                cart = await _context.Carts.Include(c=>c.CartItems)
                    .ThenInclude(c=>c.Product)
                    .FirstOrDefaultAsync(c => c.Id == cart.Id) ?? new Cart();

                var cartDto = MapCartToDto(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDto);
            }
            catch(Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unhandled exceptioin acuucred while pr" +
                    $"cessing request , Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<CartResponseDTO>> RemoveCartitemAsync(RemoveCartItemDTO removecartItemDTO)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.CartItems)
                    .ThenInclude(c => c.Product)
                    .FirstOrDefaultAsync(c => c.CustomerId == removecartItemDTO.CartItemId && !c.IsCheckedOut);

                if(cart == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Active cart not found.");
                }

                var cartItem = cart.CartItems.FirstOrDefault(c => c.Id == removecartItemDTO.CartItemId);
                
                if(cartItem == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Cart item not found.");
                }

                _context.CartItems.Remove(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                cart = await _context.Carts.Include(c => c.CartItems)
                    .ThenInclude(c => c.Product)
                    .FirstOrDefaultAsync(c => c.Id == cart.Id) ?? new Cart();

                var cartDto = MapCartToDto(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unhandled exception accured while" +
                    $"pricessing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> ClearCartAsync(int customerId)
        {
            try
            {
                var cart = await _context.Carts.Include( c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsCheckedOut);

                if(cart == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Active cart not found");
                }

                if(cart.CartItems.Any())
                {
                    _context.CartItems.RemoveRange(cart.CartItems);
                    cart.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                var confirmation = new ConfirmationResponseDTO
                {
                    Message = "Cart Has been cleared sucessfully"
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
            }
            catch (Exception ex)
            {
                return  new ApiResponse<ConfirmationResponseDTO> (500, $"An unhandled error occured " +
                    $"while processing your request, Message:{ex.Message}");
            }
        }
        private CartResponseDTO MapCartToDto(Cart cart)
        {
            var CartItemDto = cart.CartItems?.Select(ci => new CartItemResponseDTO
            {

                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                Discount = ci.Discount,
                TotalPrice = ci.TotalPrice
            }).ToList() ?? new List<CartItemResponseDTO>();

            decimal totalBasePrice = 0;
            decimal totalDiscount = 0;
            decimal totalAmount = 0;

            foreach (var item in CartItemDto)
            {
                totalBasePrice += item.UnitPrice * item.Quantity;
                totalDiscount += item.Discount * item.Quantity;
                totalAmount += item.TotalPrice;
            }
            return new CartResponseDTO
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                IsCheckedOut = cart.IsCheckedOut,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = CartItemDto,
                TotalBasePrice = totalBasePrice,
                TotalDiscount = totalDiscount,
                TotalAmount = totalAmount
            };
        }
    }
}
