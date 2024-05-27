using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RoomDTOs;

public class AddRoomDto
{
    public string RoomNumber { get; set; }
    public string Description { get; set; }
    public TypeOfRoom Type { get; set; }
    public decimal PriceOfNight { get; set; }
    public RoomStatus Status { get; set; }
    public IFormFile Photo { get; set; }
}