using System.Security.AccessControl;

namespace Domain.Entities;

public class Booking
{
    public int  Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public DateTime InDate { get; set; }
    public DateTime OutDate { get; set; }
    public string Status { get; set; }
    public Payment Payment { get; set; }
}