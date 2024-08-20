using GameInfoAPI.DTOs;

public class GameDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int AuthorId { get; set; }
    public int BestPlayerId { get; set; }
    public AuthorDTO Author { get; set; } 
    public PlayerDTO BestPlayer { get; set; } 
}