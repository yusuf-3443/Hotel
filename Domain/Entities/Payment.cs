using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;

namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int BookingId { get; set; }
    public Booking Booking { get; set; }
    public decimal Sum { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
}