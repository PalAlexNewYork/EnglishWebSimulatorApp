using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface IlibraryEnShServise
    {
        public void AddWordLibEnSh(LibraryEnShow word);
        public int GetCountWords();
        public List<LibraryEnShow> GetAll();
        public LibraryWordsJson GetWords(string word);
        public void DeleteWord(LibraryEnShow word);
        public void DeleteWord(LibraryWordsJson word);
        public void DeleteAllWords();
        public int rightAnswer { get; set; }
        public int notRightAnswer { get; set; }
        public string IdWords { get; set; }
        public void AddIdWord(int id);
        public void AddAllWordsJson(List<LibraryWordsJson> listJson);
        public List<LibraryWordsJson> GetAllWordsJson();
    }
}
