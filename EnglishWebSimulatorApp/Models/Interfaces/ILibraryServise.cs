using EnglishWebSimulatorApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILibraryServise
    {
        public List<LibraryEn> libraryEns();
        public List<LibraryEn> GetAll();
        public List<LibraryEn> GetAll(string user);
        public List<LibraryEnShow> librariesShow(List<LibraryEn> libraries);
        public void AddWord(LibraryEn en);
        public void Remove(int id);
        public void Update(LibraryEn en, string us);
        public List<Rezults> Rezults(string user);
        public void AddRezult(Rezults rezults);
        public EnglishWebSimulatorAppUser UserManager(string user);
        public List<EnglishWebSimulatorAppUser> GetUsers();
        public EnglishWebSimulatorAppUser UpdateUser(EnglishWebSimulatorAppUser user);
        public CombineResult GetRezultsWeek(string user);
        public List<LibraryEnShow> ChoiceOfWords(string check, int number, string radio, string user, string text);
        public List<UserRezultParam> GetUserRezult();
        public bool CompareWords(string word, LibraryEnShow wordObj, string lesson);
        public LibraryEn GetWordsId(int id, string user);
        public SelectList SetSelectDateTheme(string theme, string user, string name);
        public List<LibraryEnShow> GetLibrariesShowThema(string text, string user);
        public List<LibraryEnShow> GetWordsFragmentStr(string user, string text, bool flag);
        public bool WordVerification(string user, LibraryEn word);
    }
}
