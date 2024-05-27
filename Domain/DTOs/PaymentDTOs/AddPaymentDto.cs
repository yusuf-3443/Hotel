using Domain.Enums;

namespace Domain.DTOs.PaymentDTOs;

public class AddPaymentDto
{
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public decimal Sum { get; set; }
    public DateTime Date { get; set; }
    public PaymentStatus Status { get; set; }
}