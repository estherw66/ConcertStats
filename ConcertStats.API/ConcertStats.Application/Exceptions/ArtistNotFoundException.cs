namespace ConcertStats.Application.Exceptions;

public class ArtistNotFoundException : Exception
{
    public ArtistNotFoundException(int id) :
        base($"Artist with id '{id}' was not found.")
    {
    }
    
    public ArtistNotFoundException(string artistName) :
        base($"Artist with name '{artistName}' was not found.")
    {
    }
}