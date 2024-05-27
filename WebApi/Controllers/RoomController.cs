using Domain.DTOs.BookingDTOs;
using Domain.DTOs.RoomDTOs;
using Domain.Filters;
using Infrastructure.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class RoomController(IRoomService _roomService) : ControllerBase
{
    [HttpGet("Rooms")]
    [Authorize(Roles = "Admin,Staff,Guest")]
    public async Task<IActionResult> GetRooms([FromQuery] RoomFilter filter)
    {
        var res1 = await _roomService.GetRooms(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{RoomId:int}")]
    [Authorize(Roles = "Admin,Staff,Guest")]
    public async Task<IActionResult> GetRoomById(int RoomId)
    {
        var res1 = await _roomService.GetRoomById(RoomId);
        return StatusCode(res1.StatusCode, res1);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom([FromForm] AddRoomDto createRoom)
    {
        var result = await _roomService.AddRoom(createRoom);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateRoom([FromForm] UpdateRoomDto updateRoom)
    {
        var result = await _roomService.UpdateRoom(updateRoom);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{RoomId:int}")]
    public async Task<IActionResult> DeleteRoom(int RoomId)
    {
        var result = await _roomService.DeleteRoom(RoomId);
        return StatusCode(result.StatusCode, result);
    }
}