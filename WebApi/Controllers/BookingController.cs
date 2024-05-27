using Domain.DTOs.BookingDTOs;
using Domain.Filters;
using Infrastructure.Services.BookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class BookingController(IBookingService _BookingService) : ControllerBase
{
    [HttpGet("Bookings")]
    [Authorize(Roles = "Admin,Staff,Guest")]
    public async Task<IActionResult> GetBookings([FromQuery] PaginationFilter filter)
    {
        var res1 = await _BookingService.GetBookings(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{BookingId:int}")]
    [Authorize(Roles = "Admin,Staff,Guest")]
    public async Task<IActionResult> GetBookingById(int BookingId)
    {
        var res1 = await _BookingService.GetBookingById(BookingId);
        return StatusCode(res1.StatusCode, res1);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromForm] AddBookingDto createBooking)
    {
        var result = await _BookingService.AddBooking(createBooking);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateBooking([FromForm] UpdateBookingDto updateBooking)
    {
        var result = await _BookingService.UpdateBooking(updateBooking);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{BookingId:int}")]
    public async Task<IActionResult> DeleteBooking(int BookingId)
    {
        var result = await _BookingService.DeleteBooking(BookingId);
        return StatusCode(result.StatusCode, result);
    }
}