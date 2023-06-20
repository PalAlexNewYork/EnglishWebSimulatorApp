using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface IRezultsRepository
    {
        public List<Rezults> Rezults { get; }
        public void Add(Rezults rezult);
        public List<Rezults> GetRezults(string User);
    }
}
