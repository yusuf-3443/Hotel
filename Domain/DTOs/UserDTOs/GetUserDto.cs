namespace Domain.DTOs.UserDTOs;

public class GetUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegisterDate { get; set; }
}