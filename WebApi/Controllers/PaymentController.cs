using Domain.DTOs.PaymentDTOs;
using Domain.Filters;
using Infrastructure.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IPaymentService _PaymentService) : ControllerBase
{
    [HttpGet("Payments")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPayments([FromQuery] PaginationFilter filter)
    {
        var res1 = await _PaymentService.GetPayments(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{PaymentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPaymentById(int PaymentId)
    {
        var res1 = await _PaymentService.GetPaymentById(PaymentId);
        return StatusCode(res1.StatusCode, res1);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromForm] AddPaymentDto createPayment)
    {
        var result = await _PaymentService.AddPayment(createPayment);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePayment([FromForm] UpdatePaymentDto updatePayment)
    {
        var result = await _PaymentService.UpdatePayment(updatePayment);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{PaymentId:int}")]
    public async Task<IActionResult> DeletePayment(int PaymentId)
    {
        var result = await _PaymentService.DeletePayment(PaymentId);
        return StatusCode(result.StatusCode, result);
    }
}