using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressesDTOs;
using ECommerceApp.Services;    
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : Controller
    {

       private readonly AddressService _addressService;

        public AddressesController(AddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpPost("CreateAddress")]
        public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> CreateAddress([FromBody] AddressCreateDTO addressCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _addressService.CreateAddress(addressCreate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("UpdateAddress")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateAddress([FromBody] AddressUpdateDTO addressUpdate)
        {
            var response = await _addressService.UpdateAddress(addressUpdate);
            if(response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }   
            return Ok(response);
        }
        [HttpPost("GetAddressById/{id}")]
        public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> GetAddressById(int id)
        {
            var response = await _addressService.GetAddressByCustomerIdAsyn(id);
            if(response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpDelete("DeleteAddress")]
        public async Task<ActionResult<ApiResponse<List<AddressResponseDTO>>>> DeleteAddress([FromBody] AddressDeleteDTO addressDelete)
        {
            var response = await _addressService.DeleteAddress(addressDelete);
            if(response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpPost("GetAddressByCustomer/{customerId}")]
        public async Task<ActionResult<ApiResponse<List<AddressResponseDTO>>>> GetAddressByCustomer(int customerId)
        {
            var response = await _addressService.GetAddressByCustomerIdAsyn(customerId);
            if(response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
