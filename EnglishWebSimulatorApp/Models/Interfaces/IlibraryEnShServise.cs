using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface IlibraryEnShServise
    {
        public void AddWordLibEnSh(LibraryEnShow word);
        public int GetCountWords();
        public List<LibraryEnShow> GetAll();
        public void DeleteWord(LibraryEnShow word);
        public void DeleteAllWords();
        public int rightAnswer { get; set; }
        public int notRightAnswer { get; set; }
    }
}
