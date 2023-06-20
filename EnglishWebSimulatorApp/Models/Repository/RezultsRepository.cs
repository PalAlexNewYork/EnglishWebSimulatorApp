using EnglishWebSimulatorApp.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EnglishWebSimulatorApp.Models.Repository
{
    public class RezultsRepository : IRezultsRepository
    {
        public List<Rezults> Rezults { get; set; }

        public RezultsRepository(List<Rezults> rezults)
        {
            Rezults = rezults;
        }

        public void Add(Rezults rezult)=>Rezults.Add(rezult);

        public List<Rezults> GetRezults(string User) 
        {
            var list = Rezults.Where(r => r.User == User).ToList();
            return list;
        }


    }
}
