using Domain.DTOs.BookingDTOs;
using Domain.DTOs.RoomDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.BookingService;

public interface IBookingService
{
    Task<PagedResponse<List<GetBookingDto>>> GetBookings(PaginationFilter filter);
    Task<Response<GetBookingDto>> GetBookingById(int id);
    Task<Response<string>> AddBooking(AddBookingDto room);
    Task<Response<string>> UpdateBooking(UpdateBookingDto room);
    Task<Response<bool>> DeleteBooking(int id);
}