using EnglishWebSimulatorApp.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public LibraryWordsJson Update(LibraryWordsJson word)
        {
            var wordList = wordsJsons.FirstOrDefault(w => w.Id == word.Id);
            wordList.En = word.En;
            wordList.Ru = word.Ru;
            wordList.Tr = word.Tr;
            wordList.PathSound = word.PathSound;
            wordList.PathPict = word.PathPict;
            var words = wordsJsons;
            return wordList;
        }

    }
}
