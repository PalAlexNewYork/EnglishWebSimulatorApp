using EnglishWebSimulatorApp.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace EnglishWebSimulatorApp.Models.Repository
{
    public class LibraryEnRepository : ILibratyRepository
    {
        private readonly List<LibraryEn> _libraryEnList;

        public LibraryEnRepository(List<LibraryEn> libraryEnList)
        {
            _libraryEnList = libraryEnList;
        }

        public List<LibraryEn> libraries => _libraryEnList;

      
        public void Add(LibraryEn en)
        {
            bool flg = false;
            foreach (LibraryEn item in _libraryEnList) 
            {
                if (item.WordRus == en.WordRus && item.WordEng == en.WordEng) { flg = true; break; }
            }
        }

        public List<LibraryEn> GetAll() => _libraryEnList;

        public LibraryEn GetEn(string EnWord)
        {
           foreach (LibraryEn item in _libraryEnList)
                if(item.WordEng == EnWord)return item;
           return null;
        }

        public LibraryEn GetRus(string RusWord)
        {
            foreach (LibraryEn item in _libraryEnList)
                if (item.WordRus == RusWord) return item;
            return null;
        }

        public void Remove(int id)
        {
           var library = _libraryEnList.FirstOrDefault(w=>w.Id == id);
            _libraryEnList.Remove(library);
        }

        public void Update(LibraryEn en)
        {
            foreach (var i in _libraryEnList) 
            {
                if (i.WordRus == en.WordRus && i.WordEng == en.WordEng)
                { 
                i.WordRus = en.WordRus;
                i.WordEng = en.WordEng;
                i.Pict=en.Pict;
                    break;
                }
            }           
        }
    }
}
