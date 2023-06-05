using System;

namespace EnglishWebSimulatorApp.Models
{
    public class Rezults
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string User { get; set; }
        public int Words { get; set; }
        public int Error { get; set; }
        public int Gold { get; set; }

        public Rezults(int id, DateTime data, string user, int words, int error, int gold)
        {
            Id = id;
            Data = data;
            User = user;
            Words = words;
            Error = error;
            Gold = gold;
        }
        public Rezults() { }
    }
}
