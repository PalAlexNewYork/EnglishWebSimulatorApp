using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILibratyRepository
    {
        public List<LibraryEn> GetAll();
        public LibraryEn GetEn(string EnWord);
        public LibraryEn GetRus(string RusWord);
        public void Add(LibraryEn en);
        public void Remove(int id);
        public void Update(LibraryEn en);
    }
}
