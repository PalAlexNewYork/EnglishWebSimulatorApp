using EnglishWebSimulatorApp.Models;
using EnglishWebSimulatorApp.Models.Interfaces;
using EnglishWebSimulatorApp.Models.Servise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EnglishWebSimulatorApp.Controllers
{
    [Route("Lesson")]
    public class LessonController : Controller
    {
        ILibraryServise _servise;
        IlibraryEnShServise libraryEnShService;

        public LessonController(ILibraryServise servise, IlibraryEnShServise libraryEnShService)
        {
            _servise = servise;
            this.libraryEnShService = libraryEnShService;
        }
        //
        [HttpGet]
        [Route("StartLesson")]
        public IActionResult StartLesson()
        {
            return View();
        }
        //
        [HttpPost]
        [Route("AddBook")]
        public IActionResult AddBook(string check)
        {
            var libraries = _servise.librariesShow(_servise.GetAll(User.Identity.Name.ToString()));
            List<LibraryEnShow> words = new List<LibraryEnShow>();
            if (check == "tenWords")
            {
                if (libraries.Count > 10)
                {
                    var word10 = libraries.Cast<LibraryEnShow>().Reverse().Take(10).ToList();
                    libraries =word10.ToList();
                }
            }
            else if (check == "fiveWords")
            {
                if (libraries.Count > 5)
                {
                    var word5 = libraries.Cast<LibraryEnShow>().Reverse().Take(5).ToList();
                    libraries = word5.ToList();
                }
            }
            else if (check == "GetWordsAllUsers")
            {
                var librariesGetUsers = _servise.librariesShow(_servise.GetAll());
                foreach (var lib in librariesGetUsers)
                {
                    libraryEnShService.AddWordLibEnSh(lib);
                }
                return View("Lesson", librariesGetUsers);
            }
            else if (check == "CheckWordsListUser")
            {
                var wordsUser = _servise.GetAll(User.Identity.Name.ToString());
                List<MyTuple> wordsStr = new List<MyTuple>();
                foreach (var i in wordsUser) wordsStr.Add(new MyTuple(i.WordEng, i.WordRus));
                
                return View("CheckList", wordsStr);
            }
            libraryEnShService.DeleteAllWords();
            foreach (var lib in libraries) 
            {
                libraryEnShService.AddWordLibEnSh(lib);
            }
            ViewBag.right = libraryEnShService.rightAnswer;
            ViewBag.notRight = libraryEnShService.notRightAnswer;
            return View("Lesson", libraries);
        }
        //
        [HttpPost]
        [Route("LessonFormList")]
        public IActionResult LessonFormList(IEnumerable<string> groups)
        {
            var words = _servise.librariesShow(_servise.GetAll(User.Identity.Name.ToString()));
            List<LibraryEnShow> libraries = new List<LibraryEnShow>();
            foreach (var g in groups)
            {
                foreach (var w in words)
                { 
                    if(g==w.WordEng) libraries.Add(w);
                       
                }            
            }
            libraryEnShService.DeleteAllWords();
            foreach (var lib in libraries) libraryEnShService.AddWordLibEnSh(lib);  
            return View("Lesson", libraries);
        }
        //
        [HttpPost]
        [Route("Check")]
        public IActionResult Check(string word, string wordRu)
        {
            var libraries = libraryEnShService.GetAll();
            var wordObj = libraries.FirstOrDefault(w => w.WordRus == wordRu);
            bool isError = false;
            if (wordObj.WordEng.Equals(word, System.StringComparison.OrdinalIgnoreCase)) 
            {
                libraryEnShService.rightAnswer += 1;
                libraryEnShService.DeleteWord(wordObj);
                isError = true;
            }
            else
            {
                libraryEnShService.notRightAnswer += 1;
                isError = false;
            }
            var libraries_redirect = libraryEnShService.GetAll();
            ViewBag.right = libraryEnShService.rightAnswer;
            ViewBag.notRight = libraryEnShService.notRightAnswer;
            ViewBag.isError = isError;
            if (libraryEnShService.GetCountWords() == 0)
            {
                return View("EndLesson");
            }
            else
            return View("Lesson", libraries_redirect);
        }
        //
        [HttpPost]
        [Route("AddRezult") ]
        public IActionResult AddRezult() 
        {
            Rezults rezults = new Rezults();
            rezults.Words = libraryEnShService.rightAnswer;
            rezults.Error = libraryEnShService.notRightAnswer;
            rezults.User = User.Identity.Name.ToString();
            rezults.Data = DateTime.Now;
            if (libraryEnShService.notRightAnswer != 0)
                rezults.Gold = libraryEnShService.rightAnswer / libraryEnShService.notRightAnswer;
            else rezults.Gold = libraryEnShService.rightAnswer;
            rezults.Id = 0;
            if (ModelState.IsValid) 
            {
                _servise.AddRezult(rezults);
            }
            return View("StartLesson");
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
