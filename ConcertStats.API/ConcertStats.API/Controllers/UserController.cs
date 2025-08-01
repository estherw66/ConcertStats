using ConcertStats.Application.Dtos.Request.Users;
using ConcertStats.Application.Dtos.Users;
using ConcertStats.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertStats.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("username/{username}")]
    [ProducesResponseType<UserProfileDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByUsernameAsync(string username)
    {
        try
        {
            var user = await userService.GetUserProfileByUsernameAsync(username);
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<UserResultDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersAsync([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string searchQuery = "")
    {
        try
        {
            var users = await userService.GetUsersAsync(pageNumber, pageSize, searchQuery);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}