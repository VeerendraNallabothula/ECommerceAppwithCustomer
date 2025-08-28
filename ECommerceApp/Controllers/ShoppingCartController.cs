using Microsoft.AspNetCore.Mvc;
using ECommerceApp.DTOs;
using ECommerceApp.Data;
using ECommerceApp.DTOs.ShoppingCartDTOs;
using ECommerceApp.Services;

namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : Controller
    {

        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("GetCart/{customerId}")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> GetCartById(int customerId)
        {
            var response = await _shoppingCartService.GetCartByCustomerIdAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpPost("AddToCart")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> AddToCart([FromBody] AddToCartDTO addToCart)
        {
            var response = await _shoppingCartService.AddtoCartAsync(addToCart);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
     
        }
        [HttpPost("UpdateCartItem")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> UpdateCartItem([FromBody] UpdateCartItemDTO updateCart)
        {
            var response = await _shoppingCartService.UpdateCartItemAsync(updateCart);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("RemoveCartItem")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> RemoveCartItem([FromBody] RemoveCartItemDTO removeCart)
        {
            var response = await _shoppingCartService.RemoveCartitemAsync(removeCart);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("ClearCart")]
        public async Task<ActionResult<ApiResponse<CartResponseDTO>>> ClearCart([FromBody] int customerId)
        {
            var response = await _shoppingCartService.ClearCartAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

    }
}
