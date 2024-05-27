using Domain.DTOs.PaymentDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PaymentService;

public interface IPaymentService
{
    Task<PagedResponse<List<GetPaymentDto>>> GetPayments(PaginationFilter filter);
    Task<Response<GetPaymentDto>> GetPaymentById(int id);
    Task<Response<string>> AddPayment(AddPaymentDto room);
    Task<Response<string>> UpdatePayment(UpdatePaymentDto room);
    Task<Response<bool>> DeletePayment(int id);
}