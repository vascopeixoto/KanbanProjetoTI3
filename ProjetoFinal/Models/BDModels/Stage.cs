﻿namespace ProjetoFinal.Models
{
    public class Stage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Sort { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
