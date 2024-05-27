namespace Domain.Entities;

public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public decimal PriceOfNight { get; set; }
    public string Status { get; set; }
    public string Photo { get; set; }
    public List<Booking> Bookings { get; set; }
}