using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Services.HashService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seed;

public class Seeder(DataContext context, ILogger<Seeder> logger, IHashService hashService)
{

    public async Task SeedUser()
    {
        try
        {
            logger.LogInformation("Starting SeedUser in time:{DateTimeNow}", DateTime.UtcNow);
            var existing = await context.Users.AnyAsync(x => x.UserName == "admin");
            if (existing)
            {
                logger.LogWarning("User by name {Name} already exist,time {DateTimeNow}", "admin", DateTime.UtcNow);
                return;
            }

            var user = new User
            {
                Id = 1,
                Email = "rahmonyusuf3443@gmail.com",
                UserName = "admin",
                Password = hashService.ConvertToHash("1234"),
            };

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished SeedUser in time:{DateTimeNow}", DateTime.UtcNow);
            return;
        }
        catch (Exception e)
        {
            logger.LogError("Exception {EMessage}, time: {DateTimeNow}", e.Message, DateTime.UtcNow);
        }
    }

    public async Task SeedRoles()
    {
        try
        {
            logger.LogInformation("Starting SeedRoles in time:{DateTimeNow}", DateTime.UtcNow);

            var newRoles = new List<Role>()
            {
                new Role()
                {
                    Id = 1,
                    Name = Roles.Admin,
                },
                new Role()
                {
                    Id = 2,
                    Name = Roles.Staff,
                },
                new Role()
                {
                    Id = 3,
                    Name = Roles.Guest,
                },
            };

            var existingRoles = await context.Roles.ToListAsync();
            foreach (var role in newRoles)
            {
                if (existingRoles.Exists(e => e.Name == role.Name) == false)
                {
                    await context.Roles.AddAsync(role);
                }
            }

            await context.SaveChangesAsync();

            logger.LogInformation("Finished SeedRoles in time:{DateTimeNow}", DateTime.UtcNow);
            return;
        }
        catch (Exception e)
        {
            logger.LogError("Exception {EMessage}, time: {DateTimeNow}", e.Message, DateTime.UtcNow);
        }
    }

    public async Task SeedUserRole()
    {
        try
        {
            logger.LogInformation("Starting SeedUserRole in time:{DateTimeNow}", DateTime.UtcNow);
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == 1);
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == 1);
            if (user == null || role == null)
            {
                logger.LogWarning("Role {Role} or User {User} not found", "Admin", "admin");
                return;
            }

            var userRole = await context.UserRoles.AnyAsync(x => x.RoleId == 1 && x.UserId == 1);
            if (userRole)
            {
                logger.LogWarning("User in role already exists,time:{DateTimeNow}", DateTime.UtcNow);
                return;
            }

            var newUserRole = new UserRole()
            {
                RoleId = 1,
                UserId = 1,
            };
            await context.UserRoles.AddAsync(newUserRole);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished SeedUserRole in time:{DateTimeNow}", DateTime.UtcNow);
            return;
        }
        catch (Exception e)
        {
            logger.LogError("Exception {EMessage}, time: {DateTimeNow}", e.Message, DateTime.UtcNow);
        }
    }

}