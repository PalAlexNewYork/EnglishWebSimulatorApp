using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILybraryWorkJsonRepository
    {
        public List<LibraryWordsJson> GetAll();
        public List<LibraryWordsJson> GetWordsHand(IEnumerable<string> groups);
        public LibraryWordsJson Create(LibraryWordsJson category);
        public LibraryWordsJson Update(LibraryWordsJson word);
        public LibraryWordsJson Delete(int id);
    }
}
