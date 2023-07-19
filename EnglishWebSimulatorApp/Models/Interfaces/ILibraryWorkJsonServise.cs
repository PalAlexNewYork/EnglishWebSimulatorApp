using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILibraryWorkJsonServise
    {
        public List<LibraryWordsJson> GetAllWords();
        public List<LibraryWordsJson> SelectWordsServise( string check, int number, string str, int numberOne, int numberTo);
        public List<LibraryWordsJson> GetWordsHand(IEnumerable<string> groups);
        public bool ComparedWords(string word, string wordRight);
        public string GetWordsEnStr(string idWords);
        public LibraryWordsJson AddWord(LibraryWordsJson word);
        public LibraryWordsJson UpdateWord(LibraryWordsJson word);
        public List<LibraryWordsJson> GetFragmentWords(string str, bool isCyrilic);
        public List<LibraryWordsJson> SearchFromToId(int numberFrom, int nubberTo);
    }
}
