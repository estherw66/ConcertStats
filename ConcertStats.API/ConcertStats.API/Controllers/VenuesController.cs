using ConcertStats.Application.Dtos.Request.Venues;
using ConcertStats.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertStats.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VenuesController(IVenueService venueService, ILogger<VenuesController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateVenueAsync([FromBody] CreateVenueRequest request)
    {
        try
        {
            var venue = await venueService.CreateVenueAsync(request);
            logger.LogInformation("Create venue with id {VenueId}", venue.Id);
            return CreatedAtAction("GetVenueById", new { id = venue.Id }, venue);
        }

        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
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
            logger.LogError(ex, "Error creating new venue");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected problem occurred.");
        }
    }

    [HttpGet("{id:int}", Name = "GetVenueById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVenueByIdAsync(int id)
    {
        try
        {
            var venue = await venueService.GetVenueByIdAsync(id);
            return Ok(venue);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVenueByNameAsync(string name)
    {
        try
        {
            var venue = await venueService.GetVenueByNameAsync(name);
            return Ok(venue);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVenueAsync(int id, [FromBody] UpdateVenueRequest request)
    {
        try
        {
            await venueService.UpdateVenueAsync(id, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NullReferenceException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVenueAsync(int id)
    {
        try
        {
            await venueService.DeleteVenueAsync(id);
            return NoContent();
        }
        catch (NullReferenceException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting the venue.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}