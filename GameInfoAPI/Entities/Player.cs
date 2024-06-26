using System.Collections.Generic;

namespace GameInfoAPI.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}