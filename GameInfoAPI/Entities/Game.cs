namespace GameInfoAPI.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public int GameAgeRestriction { get; set; } = 0;
        public string GameDescription { get; set; } = string.Empty;
        public Author Author { get; set; } = new Author();
        public List<Player> Players { get; set; } = new List<Player>();
    }
}