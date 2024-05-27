namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Code { get; set; }
    public DateTimeOffset CodeTime { get; set; }
    public DateTime RegisterDate { get; set; }
    public List<Booking> Bookings { get; set; }
    public List<UserRole> UserRoles { get; set; }
    public List<Payment> Payments { get; set; }
}