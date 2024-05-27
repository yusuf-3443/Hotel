using System.Net;
using AutoMapper;
using Domain.DTOs.RoomDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.RoomService 
{
    public class RoomService : IRoomService 
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;
        private readonly IFileService _fileService;

        public RoomService(DataContext context, IMapper mapper, ILogger<RoomService> logger, IFileService fileService) 
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<PagedResponse<List<GetRoomDto>>> GetRooms(RoomFilter filter) 
        {
            try
            {
                _logger.LogInformation("Starting method GetRooms in time: {DateTimeNow}", DateTime.Now);
                var rooms = _context.Rooms.AsQueryable(); 
                var result = await rooms.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                    .ToListAsync();
                var total = await rooms.CountAsync();
                var response = _mapper.Map<List<GetRoomDto>>(result);
                _logger.LogInformation("Finished method GetRooms in time: {DateTimeNow}", DateTime.Now);
                return new PagedResponse<List<GetRoomDto>>(response, total, filter.PageNumber, filter.PageSize);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method GetRooms in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new PagedResponse<List<GetRoomDto>>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<GetRoomDto>> GetRoomById(int id) 
        {
            try
            {
                _logger.LogInformation("Starting method GetRoomById in time: {DateTimeNow}", DateTime.Now);
                var exist = await _context.Rooms.FindAsync(id); 
                if (exist == null) return new Response<GetRoomDto>(HttpStatusCode.BadRequest, "Not found");
                var mapped = _mapper.Map<GetRoomDto>(exist);
                _logger.LogInformation("Finished method GetRoomById in time: {DateTimeNow}", DateTime.Now);
                return new Response<GetRoomDto>(mapped);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method GetRoomById in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<GetRoomDto>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<string>> AddRoom(AddRoomDto room) 
        {
            try
            {
                _logger.LogInformation("Starting method AddRoom in time: {DateTimeNow}", DateTime.Now);

                var room1 = new Room()
                {
                    RoomNumber = room.RoomNumber,
                    Description = room.Description,
                    PriceOfNight = room.PriceOfNight,
                    Photo = await _fileService.CreateFile(room.Photo)
                };
                if (room.Status == Domain.Enums.RoomStatus.Available) room1.Status = "Available";
                if (room.Status == Domain.Enums.RoomStatus.Booked) room1.Status = "Booked";
                if (room.Status == Domain.Enums.RoomStatus.Occupied) room1.Status = "Occupied";
                if (room.Type == Domain.Enums.TypeOfRoom.Single) room1.Type = "Single";
                if (room.Type == Domain.Enums.TypeOfRoom.Double) room1.Type = "Double";
                if (room.Type == Domain.Enums.TypeOfRoom.Suite) room1.Type = "Suite";
                
                await _context.Rooms.AddAsync(room1);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Finished method AddRoom in time: {DateTimeNow}", DateTime.Now);
                return new Response<string>("Successfully added");
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method AddRoom in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<string>> UpdateRoom(UpdateRoomDto room)
        {
            try
            {
                _logger.LogInformation("Starting method UpdateRoom in time: {DateTimeNow}", DateTime.Now);
                var existing = await _context.Rooms.AnyAsync(x => x.Id == room.Id); 
                if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Not found");
                var mapped = _mapper.Map<Room>(room); 
                _context.Rooms.Update(mapped); 
                await _context.SaveChangesAsync();
                _logger.LogInformation("Finished method UpdateRoom in time: {DateTimeNow}", DateTime.Now);
                return new Response<string>("Updated Successfully");
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method UpdateRoom in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<bool>> DeleteRoom(int id) 
        {
            try
            {
                _logger.LogInformation("Starting method DeleteRoom in time: {DateTimeNow}", DateTime.Now);
                var existing = await _context.Rooms.Where(x => x.Id == id).ExecuteDeleteAsync(); 
                if (existing == 0) return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
                _logger.LogInformation("Finished method DeleteRoom in time: {DateTimeNow}", DateTime.Now);
                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in method DeleteRoom in time: {DateTimeNow}\nError: {EMessage}",
                    DateTime.Now, e.Message);
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
