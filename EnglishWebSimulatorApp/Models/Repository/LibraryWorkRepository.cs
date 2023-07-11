using EnglishWebSimulatorApp.Models.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EnglishWebSimulatorApp.Models.Repository
{
    public class LibraryWorkRepository : ILybraryWorkJsonRepository
    {
        public List<LibraryWordsJson> wordsJsons = new List<LibraryWordsJson>();

        public LibraryWorkRepository(List<LibraryWordsJson> wordsJsons)
        {
            this.wordsJsons = wordsJsons;
        }

        public List<LibraryWordsJson> GetWordsHand(IEnumerable<string> groups) 
        {
            List < LibraryWordsJson > tmp = new List<LibraryWordsJson>();
            foreach (var group in groups) 
            {
                foreach(var word in wordsJsons) 
                {
                    if (word.En == group) { tmp.Add(word); break; }
                }
            }
            return tmp;
        }
        public LibraryWordsJson Create(LibraryWordsJson category) =>throw new System.NotImplementedException();

        public LibraryWordsJson Delete(int id) => throw new System.NotImplementedException();

        public List<LibraryWordsJson> GetAll() => wordsJsons;

        public LibraryWordsJson Update(LibraryWordsJson category) => throw new System.NotImplementedException();
    }
}
