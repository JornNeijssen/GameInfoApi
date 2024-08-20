using System;
using System.ComponentModel.DataAnnotations;

namespace GameInfoAPI.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; } 
        public int AuthorId { get; set; } 
        public Author Author { get; set; }
        public int BestPlayerId { get; set; }
        public Player BestPlayer { get; set; } 
    }
}