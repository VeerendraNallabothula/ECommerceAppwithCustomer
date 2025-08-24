using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressesDTOs;
namespace ECommerceApp.DTOs.AddressesDTOs
{
    public class AddressService
    {
        private readonly ApplicationDbContext _context;

        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<AddressResponseDTO>> CreateAddress(AddressCreateDTO addressCreate)
        {
            try
            {


                var customer = _context.Customers.FindAsync(addressCreate.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Customer not found");
                }
                var address = new Address
                {
                    CustomerId = addressCreate.CustomerId,
                    AddressLine1 = addressCreate.AddressLine1,
                    AddressLine2 = addressCreate.AddressLine2,
                    City = addressCreate.City,
                    State = addressCreate.State,
                    PostalCode = addressCreate.PostalCode,
                    Country = addressCreate.Country
                };

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                var addressResponse = new AddressResponseDTO
                {
                    Id = address.id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                };
                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddress(AddressUpdateDTO addressUpdate)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync( a => a.id == addressUpdate.AddressId && a.CustomerId == addressUpdate.CustomerId);
                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found");
                }

                address.AddressLine1 = addressUpdate.AddressLine1;
                address.AddressLine2 = addressUpdate.AddressLine2;
                address.City = addressUpdate.City;
                address.State = addressUpdate.State;
                address.PostalCode = addressUpdate.PostalCode;
                address.Country = addressUpdate.Country;
                _context.Addresses.Update(address);
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressUpdate.AddressId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddress(AddressDeleteDTO addressDelete)
        {
            try
            {
                var address = await _context.Addresses.FindAsync(addressDelete.AddressId);
                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found");
                }
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDelete.AddressId} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<AddressResponseDTO>>> GetAddressByCustomerIdAsyn(int customerId)
        {
            try
            {
                var customer = await _context.Customers.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                {
                    return new ApiResponse<List<AddressResponseDTO>>(404, "Customer not found");
                }
                var addresses = customer.Addresses.Select(address => new AddressResponseDTO
                {
                    Id = address.id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                }).ToList();
                return new ApiResponse<List<AddressResponseDTO>>(200, addresses);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AddressResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
