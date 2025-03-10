using Domain.DTOs.UserRoleDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.UserRoleService;

public interface IUserRoleService
{
    Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync(PaginationFilter filter);
    Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(GetUserRoleDto userRoleDto);
    Task<Response<string>> CreateUserRoleAsync(AddUserRoleDto createUserRole);
    Task<Response<bool>> DeleteUserRoleAsync(AddUserRoleDto userRoleId);
}