using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckDating.Backend.Data;

namespace TruckDating.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Select(user => new
            {
                user.Id,
                user.Email,
                user.Status,
                user.EmailVerified,
                user.CreatedAt
            })
            .ToListAsync();

        return Ok(users);
    }
}