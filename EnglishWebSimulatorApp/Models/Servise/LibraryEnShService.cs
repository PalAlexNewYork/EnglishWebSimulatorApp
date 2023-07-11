using EnglishWebSimulatorApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace EnglishWebSimulatorApp.Models.Servise
{
    public class LibraryEnShService : IlibraryEnShServise
    {
        public List<LibraryEnShow> libraryEnShows;
        public List<LibraryWordsJson> wordsJson;
        public int rightAnswer { get; set; }
        public int notRightAnswer { get; set; }
        public string IdWords { get; set; }
        public LibraryEnShService()
        {
            this.libraryEnShows = new List<LibraryEnShow>();
            this.rightAnswer = 0;
            this.notRightAnswer = 0;
            this.wordsJson = new List<LibraryWordsJson>();
        }
        public void AddWordLibEnSh(LibraryEnShow word)=>libraryEnShows.Add(word);
        public void AddAllWordsJson(List<LibraryWordsJson> listJson) 
        { 
            this.wordsJson = listJson;
            foreach (var i in listJson) this.AddIdWord(i.Id);
        }
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
            this.wordsJson.Clear();
            rightAnswer = 0;
            notRightAnswer = 0;
            IdWords = "";
            //IdWords.RemoveAll(x=>x>=0);
        }
        public void AddIdWord(int id)
        { 
            IdWords = IdWords +"%"+ id.ToString();
        }
        public void DeleteWord(LibraryWordsJson word)=>this.wordsJson.Remove(word);
        public LibraryWordsJson GetWords(string word) => this.wordsJson.FirstOrDefault(w => w.En == word);
        public List<LibraryWordsJson> GetAllWordsJson() => this.wordsJson;
        
    }
}
