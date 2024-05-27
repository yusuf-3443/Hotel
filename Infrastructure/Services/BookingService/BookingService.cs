using System.Net;
using AutoMapper;
using Domain.DTOs.BookingDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(DataContext context, IMapper mapper, ILogger<BookingService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<List<GetBookingDto>>> GetBookings(PaginationFilter filter)
        {
            try
            {
                _logger.LogInformation("Starting method GetBookings in time: {DateTimeNow}", DateTime.Now);
                var bookings = _context.Bookings.AsQueryable();
                var result = await bookings.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                    .ToListAsync();
                var total = await bookings.CountAsync();
                var response = _mapper.Map<List<GetBookingDto>>(result);
                _logger.LogInformation("Finished method GetBookings in time: {DateTimeNow}", DateTime.Now);
                return new PagedResponse<List<GetBookingDto>>(response, total, filter.PageNumber, filter.PageSize);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method GetBookings in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new PagedResponse<List<GetBookingDto>>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<GetBookingDto>> GetBookingById(int id)
        {
            try
            {
                _logger.LogInformation("Starting method GetBookingById in time: {DateTimeNow}", DateTime.Now);
                var exist = await _context.Bookings.FindAsync(id);
                if (exist == null) return new Response<GetBookingDto>(HttpStatusCode.BadRequest, "Not found");
                var mapped = _mapper.Map<GetBookingDto>(exist);
                _logger.LogInformation("Finished method GetBookingById in time: {DateTimeNow}", DateTime.Now);
                return new Response<GetBookingDto>(mapped);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method GetBookingById in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<GetBookingDto>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<string>> AddBooking(AddBookingDto booking)
        {
            try
            {
                _logger.LogInformation("Starting method AddBooking in time: {DateTimeNow}", DateTime.Now);
                var mapped = _mapper.Map<Booking>(booking);
                await _context.Bookings.AddAsync(mapped);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Finished method AddBooking in time: {DateTimeNow}", DateTime.Now);
                return new Response<string>("Successfully added");
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method AddBooking in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<string>> UpdateBooking(UpdateBookingDto booking)
        {
            try
            {
                _logger.LogInformation("Starting method UpdateBooking in time: {DateTimeNow}", DateTime.Now);
                var existing = await _context.Bookings.AnyAsync(x => x.Id == booking.Id);
                if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Not found");
                var mapped = _mapper.Map<Booking>(booking);
                _context.Bookings.Update(mapped);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Finished method UpdateBooking in time: {DateTimeNow}", DateTime.Now);
                return new Response<string>("Updated Successfully");
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method UpdateBooking in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<bool>> DeleteBooking(int id)
        {
            try
            {
                _logger.LogInformation("Starting method DeleteBooking in time: {DateTimeNow}", DateTime.Now);
                var existing = await _context.Bookings.Where(x => x.Id == id).ExecuteDeleteAsync();
                if (existing == 0) return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                _logger.LogInformation("Finished method DeleteBooking in time: {DateTimeNow}", DateTime.Now);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method DeleteBooking in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
