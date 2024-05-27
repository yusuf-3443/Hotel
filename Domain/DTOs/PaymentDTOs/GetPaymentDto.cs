namespace Domain.DTOs.PaymentDTOs;

public class GetPaymentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public decimal Sum { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
}