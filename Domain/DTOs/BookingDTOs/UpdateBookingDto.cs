using Domain.Enums;

namespace Domain.DTOs.BookingDTOs;

public class UpdateBookingDto
{
    public int  Id { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateTime InDate { get; set; }
    public DateTime OutDate { get; set; }
    public BookingStatus Status { get; set; }
}