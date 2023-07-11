using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILibraryWorkJsonServise
    {
        public List<LibraryWordsJson> GetAllWords();
        public List<LibraryWordsJson> SelectWordsServise( string check, int number, string str, int numberOne, int numberTo);
        public List<LibraryWordsJson> GetWordsHand(IEnumerable<string> groups);
        public bool ComparedWords(string word, string wordRight);
        public IRezultsRepository RezultsRepository { get; set; }
        public string GetWordsEnStr(string idWords);
    }
}
