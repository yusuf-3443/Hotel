using System.Net;
using AutoMapper;
using Domain.DTOs.PaymentDTOs; // Update namespace to PaymentDTOs
using Domain.Entities; // Ensure you have the Payment entity defined in this namespace
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.PaymentService // Update namespace and class name
{
    public class PaymentService : IPaymentService // Update class name to PaymentService and implement IPaymentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger; // Update logger type

        public PaymentService(DataContext context, IMapper mapper, ILogger<PaymentService> logger) // Update constructor parameters
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<List<GetPaymentDto>>> GetPayments(PaginationFilter filter) // Update method name to GetPayments
        {
            try
            {
                _logger.LogInformation("Starting method GetPayments in time: {DateTimeNow}", DateTime.Now);
                var payments = _context.Payments.AsQueryable(); // Update entity name to Payments
                var result = await payments.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                    .ToListAsync();
                var total = await payments.CountAsync();
                var response = _mapper.Map<List<GetPaymentDto>>(result);
                _logger.LogInformation("Finished method GetPayments in time: {DateTimeNow}", DateTime.Now);
                return new PagedResponse<List<GetPaymentDto>>(response, total, filter.PageNumber, filter.PageSize);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method GetPayments in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new PagedResponse<List<GetPaymentDto>>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<GetPaymentDto>> GetPaymentById(int id) // Update method name to GetPaymentById
        {
            try
            {
                _logger.LogInformation("Starting method GetPaymentById in time: {DateTimeNow}", DateTime.Now);
                var exist = await _context.Payments.FindAsync(id); // Update entity name to Payments
                if (exist == null) return new Response<GetPaymentDto>(HttpStatusCode.BadRequest, "Not found");
                var mapped = _mapper.Map<GetPaymentDto>(exist);
                _logger.LogInformation("Finished method GetPaymentById in time: {DateTimeNow}", DateTime.Now);
                return new Response<GetPaymentDto>(mapped);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method GetPaymentById in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<GetPaymentDto>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<string>> AddPayment(AddPaymentDto payment) // Update method name to AddPayment
        {
            try
            {
                _logger.LogInformation("Starting method AddPayment in time: {DateTimeNow}", DateTime.Now);
                var mapped = _mapper.Map<Payment>(payment); // Update entity name to Payment
                await _context.Payments.AddAsync(mapped); // Update entity name to Payments
                await _context.SaveChangesAsync();
                _logger.LogInformation("Finished method AddPayment in time: {DateTimeNow}", DateTime.Now);
                return new Response<string>("Successfully added");
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method AddPayment in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<string>> UpdatePayment(UpdatePaymentDto payment) // Update method name to UpdatePayment
        {
            try
            {
                _logger.LogInformation("Starting method UpdatePayment in time: {DateTimeNow}", DateTime.Now);
                var existing = await _context.Payments.AnyAsync(x => x.Id == payment.Id); // Update entity name to Payments
                if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Not found");
                var mapped = _mapper.Map<Payment>(payment); // Update entity name to Payment
                _context.Payments.Update(mapped); // Update entity name to Payments
                await _context.SaveChangesAsync();
                _logger.LogInformation("Finished method UpdatePayment in time: {DateTimeNow}", DateTime.Now);
                return new Response<string>("Updated Successfully");
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method UpdatePayment in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<bool>> DeletePayment(int id) // Update method name to DeletePayment
        {
            try
            {
                _logger.LogInformation("Starting method DeletePayment in time: {DateTimeNow}", DateTime.Now);
                var existing = await _context.Payments.Where(x => x.Id == id).ExecuteDeleteAsync(); // Update entity name to Payments
                if (existing == 0) return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                _logger.LogInformation("Finished method DeletePayment in time: {DateTimeNow}", DateTime.Now);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method DeletePayment in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
