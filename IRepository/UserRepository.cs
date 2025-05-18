// Repository/UserRepository.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;

namespace mvc.IRepository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Users>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Users?> GetUserByEmailAsync(string normalizedEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
    }

    public async Task<Users?> GetUserByIdAsync(string id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddUserAsync(Users user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UserExistsAsync(string id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task UpdateUserAsync(Users user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
        {
            throw new Exception("User not found.");
        }

        // Update only the necessary fields
        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email;
        existingUser.accountType = user.accountType;
        existingUser.subscriptionId = user.subscriptionId;
        existingUser.photoPath = user.photoPath;
        existingUser.NormalizedEmail = user.NormalizedEmail;
        existingUser.NormalizedUserName = user.NormalizedUserName;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Users user)
    {
        // Remove user roles
        var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id);
        _context.UserRoles.RemoveRange(userRoles);

        // Remove the user
        _context.Users.Remove(user);

        // Save changes to the database
        await _context.SaveChangesAsync();
    }

    public async Task AddUserRoleAsync(IdentityUserRole<string> userRole)
    {
        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserRoleAsync(Users user, string newRole)
    {
        // Fetch the current roles of the user
        var currentRoles = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .ToListAsync();

        // Remove existing roles
        if (currentRoles.Any())
        {
            _context.UserRoles.RemoveRange(currentRoles);
        }

        // Fetch the role ID for the new role
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == newRole);
        if (role == null)
        {
            throw new Exception($"Role '{newRole}' does not exist.");
        }

        // Add the new role
        var newUserRole = new IdentityUserRole<string>
        {
            UserId = user.Id,
            RoleId = role.Id
        };
        _context.UserRoles.Add(newUserRole);

        // Save changes to the database
        await _context.SaveChangesAsync();
    }
    
    public async Task AddUserRoleAsync(string userId, string roleId)
    {
        var userRole = new IdentityUserRole<string>
        {
            UserId = userId,
            RoleId = roleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task<Users?> GetUserWithSubscriptionAsync(string userId)
    {
        return await _context.Users
            .Include(u => u.subscription)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

}
