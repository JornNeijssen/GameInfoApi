﻿using System.Collections.Generic;

namespace GameInfoAPI.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}