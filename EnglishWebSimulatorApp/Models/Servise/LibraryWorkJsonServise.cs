using EnglishWebSimulatorApp.Data;
using EnglishWebSimulatorApp.Models.Interfaces;
using EnglishWebSimulatorApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace EnglishWebSimulatorApp.Models.Servise
{
    public class LibraryWorkJsonServise : ILibraryWorkJsonServise
    {
        private readonly EnglishWebSimulatorAppContext context;
        private ILibraryWorkJson libraryWork  {get;}
        public ILybraryWorkJsonRepository workJsonRepository { get; set; }
        private IRezultsRepository rezultsRepository;
        public LibraryWorkJsonServise(ILibraryWorkJson LibraryWorkJson)
        {
            this.libraryWork = LibraryWorkJson;
            workJsonRepository = this.libraryWork.workJsonRepository;
        }

        public IRezultsRepository RezultsRepository { get; set; }

        public List<LibraryWordsJson> GetAllWords() => workJsonRepository.GetAll();
        public List<LibraryWordsJson> SelectWordsServise(string check, int number, string str, int numberFrom, int numberTo) 
        {
            List<LibraryWordsJson> words = new List<LibraryWordsJson>();
            var list = workJsonRepository.GetAll();
            if (check == "NumberWords")
            {
                Random random = new Random();
                for (var i = 0; i < number;)
                {
                    int num = random.Next(workJsonRepository.GetAll().Count - 1);
                    if (!words.Any(x => x == list[num])) words.Add(list[num]); i++;
                }
            }
            else if (check == "IdNumberWords") 
            {
                if ((numberTo - numberFrom) > 3 && numberTo<workJsonRepository.GetAll().Count)
                {
                    for(int i = numberFrom; i<=numberTo; i++)
                    {
                        words.Add(workJsonRepository.GetAll()[i]);
                    }
                }
                else return words;
            }
            else
            {
                if (!string.IsNullOrEmpty(str))
                {
                    words = list.Where(w => w.En.Contains(str) || w.Ru.Contains(str)).ToList();
                }
            }
            return words;
        }
        public List<LibraryWordsJson> GetWordsHand(IEnumerable<string> groups)=>workJsonRepository.GetWordsHand(groups).ToList();
        public bool ComparedWords(string word, string wordRight) 
        {
            if (wordRight.Equals(word, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }else 
                return false;
        }

        public string GetWordsEnStr(string idWords)
        {
            var idArr = idWords.Split("%");
            var str = "";
            foreach (var id in idArr)
            {
                foreach (var word in workJsonRepository.GetAll())
                {
                    if (id != "") 
                    {
                    int ind = Convert.ToInt32(id);
                    if (ind == word.Id) { str = str  + word.En+ ", "; break; }
                    }            
                }
            }
            return str;
        }

    }
}
