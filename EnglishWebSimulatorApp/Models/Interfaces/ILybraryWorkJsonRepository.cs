using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILybraryWorkJsonRepository
    {
        public List<LibraryWordsJson> GetAll();
        public List<LibraryWordsJson> GetWordsHand(IEnumerable<string> groups);
        public LibraryWordsJson Create(LibraryWordsJson category);
        public LibraryWordsJson Update(LibraryWordsJson category);
        public LibraryWordsJson Delete(int id);
    }
}
