using EnglishWebSimulatorApp.Models.Interfaces;
using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Servise
{
    public class LibraryEnShService : IlibraryEnShServise
    {
        public List<LibraryEnShow> libraryEnShows;
        public int rightAnswer { get; set; }
        public int notRightAnswer { get; set; }

        public LibraryEnShService()
        {
            this.libraryEnShows = new List<LibraryEnShow>();
            this.rightAnswer = 0;
            this.notRightAnswer = 0;
        }

        public void AddWordLibEnSh(LibraryEnShow word)=>libraryEnShows.Add(word);

        public void DeleteWord(LibraryEnShow w)
        {
            libraryEnShows.Remove(w);
        }

        public List<LibraryEnShow> GetAll() => libraryEnShows;

        public int GetCountWords()
        {
            return libraryEnShows.Count;
        }
        public void DeleteAllWords()
        {
            libraryEnShows.Clear();
            rightAnswer = 0;
            notRightAnswer = 0;
        }
    }
}
