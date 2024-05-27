using Domain.DTOs.RoomDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.RoomService;

public interface IRoomService
{
    Task<PagedResponse<List<GetRoomDto>>> GetRooms(RoomFilter filter);
    Task<Response<GetRoomDto>> GetRoomById(int id);
    Task<Response<string>> AddRoom(AddRoomDto room);
    Task<Response<string>> UpdateRoom(UpdateRoomDto room);
    Task<Response<bool>> DeleteRoom(int id);
}