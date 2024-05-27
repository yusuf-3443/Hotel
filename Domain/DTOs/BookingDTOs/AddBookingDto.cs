
using Domain.Enums;

namespace Domain.DTOs.BookingDTOs;

public class AddBookingDto
{
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateTime InDate { get; set; }
    public DateTime OutDate { get; set; }
    public BookingStatus Status { get; set; }
}