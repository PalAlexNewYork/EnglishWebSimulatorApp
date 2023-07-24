using EnglishWebSimulatorApp.Models;
using EnglishWebSimulatorApp.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnglishWebSimulatorApp.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        //private readonly ILibraryServise _libraryServise;
        private readonly ILibraryWorkJsonServise library;

        public AdminController(ILibraryWorkJsonServise library)
        {
            //_libraryServise = libraryServise;
            this.library = library;
        }
        [HttpGet]
        [Route("AdminsMainPage")]
        public IActionResult AdminsMainPage() 
        {
            return View();
        }
        //
        [HttpGet]
        [Route("ShowWordsDictionary")]
        public IActionResult ShowWordsDictionary()
        {
                var words = this.library.GetAllWords();
                ViewBag.TypeAction = "show";
                return View(words);
        }
        //
        [HttpGet]
        [Route("ShowCardsWords")]
        public IActionResult ShowCardsWords(int id, string flag) 
        {
            var word = this.library.GetNextOrIdWord(id, flag);
            return View(word);
        }
        //
        [HttpGet]
        [Route("AddWordDictionary")]
        public IActionResult AddWordDictionary(string messege, int objId) 
        {
            var obj = this.library.GetAllWords().FirstOrDefault(w => w.Id == objId);
            ViewBag.Mes = messege;
            return View(obj);
        }
        //
        [HttpPost]
        [Route("AddWordJson")]
        public async Task<IActionResult> AddWordJson(LibraryWordsJson word, IFormFile file, IFormFile fileOne) 
        {
            if (file != null && file.Length != 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/img/words/", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create)) await file.CopyToAsync(stream);
                word.PathPict = Path.GetFileName(file.FileName);
            }
            if (fileOne != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/sounds/", fileOne.FileName);
                using (var stream = new FileStream(path, FileMode.Create)) await fileOne.CopyToAsync(stream);
                word.PathSound = Path.GetFileName(fileOne.FileName);
            }
            if (word.Id == 0)
            {
                    if (this.library.AddWord(word)!=null)
                        return Redirect("ShowWordsDictionary");
                    else
                    {
                        string mesg = "HasAlready";
                        return Redirect($"AddWordDictionary?messege={mesg}");
                    }
            }
            else 
            {
                this.library.UpdateWord(word);
                return Redirect("ShowWordsDictionary");
            }
            string mes = "Error";
                    return Redirect($"AddWordDictionary?messege={mes}");
        }
        //
        [HttpGet]
        [Route("SearchFragmentStr")]
        public IActionResult SearchFragmentStr(string str) 
        {
            if (!string.IsNullOrEmpty(str)) 
            {
                string pattern = @"\p{IsCyrillic}";               
                var list = library.GetFragmentWords(str, (Regex.Matches(str, pattern).Count > 0));
                return View("ShowWordsDictionary", list);
            }
            return Redirect("ShowWordsDictionary");
        }
        //
        [HttpGet]
        [Route("SearchFromToId")]
        public IActionResult SearchFromToId(int numberFrom, int numberTo) 
        {
            if (numberFrom < numberTo) 
            {
                var list = library.SearchFromToId(numberFrom, numberTo);
                return View("ShowWordsDictionary", list);
            }
            else 
                return Redirect("ShowWordsDictionary");
        }
        public IActionResult Index()
        {
            return View();
        }
        //

        //
        public bool isCyrillic(string textInput)
        {
            bool rezultat = true;
            string pattern = @"[абвгдѓежзѕијклљмнњопрстќуфхцчџш]";
            char[] textArray = textInput.ToCharArray();
            for (int i = 0; i < textArray.Length; i++)
            {
                if (!Regex.IsMatch(textArray[i].ToString(), pattern))
                {
                    rezultat = false;
                    break;
                }
            }
            return rezultat;
        }
    }
}
