using ConcertStats.Application.Dtos.Request.Artists;
using ConcertStats.Application.Exceptions;
using ConcertStats.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertStats.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController(IArtistService artistService, ILogger<ArtistsController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateArtistAsync([FromBody] CreateArtistRequest request)
    {
        try
        {
            var artist = await artistService.CreateArtistAsync(request);
            logger.LogInformation($"Created artist with id {artist.Id}");
            return CreatedAtAction("GetArtistById", new { id = artist.Id }, artist);
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
            logger.LogError(ex, "An error occurred while creating the artist.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id:int}", Name = "GetArtistById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArtistByIdAsync(int id)
    {
        try
        {
            var artist = await artistService.GetArtistByIdAsync(id);
            return Ok(artist);
        }
        catch (ArtistNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArtistByNameAsync(string name)
    {
        try
        {
            var artist = await artistService.GetArtistByNameAsync(name);
            return Ok(artist);
        }
        catch (ArtistNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateArtistAsync(int id, [FromBody] UpdateArtistRequest request)
    {
        try
        {
            await artistService.UpdateArtistAsync(id, request);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArtistNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArtistAsync(int id)
    {
        try
        {
            await artistService.DeleteArtistAsync(id);
            return NoContent();
        }
        catch (ArtistNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}