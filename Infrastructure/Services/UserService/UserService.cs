using System.Net;
using Domain.DTOs.UserDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.UserService;

public class UserService(ILogger<UserService> logger, DataContext context) : IUserService
{

    public async Task<PagedResponse<List<GetUserDto>>> GetUsersAsync(UserFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetUsersAsync} in time:{DateTime} ", "GetUsersAsync",
                DateTimeOffset.UtcNow);

            var users = context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Username))
                users = users.Where(x => x.UserName.ToLower().Contains(filter.Username.ToLower()));

            var response = await users.Select(x => new GetUserDto()
            {
                Email = x.Email,
                UserName = x.UserName,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await users.CountAsync();

            logger.LogInformation("Finished method {GetUsersAsync} in time:{DateTime} ", "GetUsersAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetUserDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetUserDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetUserDto>> GetUserByIdAsync(int userId)
    {
        try
        {
            logger.LogInformation("Starting method {GetUserByIdAsync} in time:{DateTime} ", "GetUserByIdAsync",
                DateTimeOffset.UtcNow);
            var user = await context.Users.Select(x => new GetUserDto()
            {
                Email = x.Email,
                UserName = x.UserName,
                Id = x.Id,
            }).FirstOrDefaultAsync(x => x.Id == userId);

            logger.LogInformation("Finished method {GetUserByIdAsync} in time:{DateTime} ", "GetUserByIdAsync",
                DateTimeOffset.UtcNow);
            return user == null
                ? new Response<GetUserDto>(HttpStatusCode.BadRequest, $"User not found by ID:{userId}")
                : new Response<GetUserDto>(user);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUser, int userId)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateUserAsync} in time:{DateTime} ", "UpdateUserAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Users.Where(x => x.Id == userId)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.Email, updateUser.Email)
                    .SetProperty(u => u.UserName, updateUser.UserName)
                );

            logger.LogInformation("Finished method {UpdateUserAsync} in time:{DateTime} ", "UpdateUserAsync",
                DateTimeOffset.UtcNow);
            return existing == 0
                ? new Response<string>(HttpStatusCode.BadRequest, "Invalid request ")
                : new Response<string>("Successfully updated ");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

}