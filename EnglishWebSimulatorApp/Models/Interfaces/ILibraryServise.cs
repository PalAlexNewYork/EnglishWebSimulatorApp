using EnglishWebSimulatorApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILibraryServise
    {
        public List<LibraryEn> GetAll();
        public List<LibraryEn> GetAll(string user);
        public List<LibraryEnShow> librariesShow(List<LibraryEn> libraries);
        public void AddWord(LibraryEn en);
        public void Remove(int id);
        public void Update(LibraryEn en, string us);
        public List<Rezults> Rezults(string user);
        public void AddRezult(Rezults rezults);
        public EnglishWebSimulatorAppUser UserManager(string user);
        public EnglishWebSimulatorAppUser UpdateUser(EnglishWebSimulatorAppUser user);


    }
}
