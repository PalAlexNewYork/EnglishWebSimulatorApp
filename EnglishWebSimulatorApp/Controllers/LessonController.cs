using EnglishWebSimulatorApp.Models;
using EnglishWebSimulatorApp.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        //
        [HttpGet]
        [Route("UserProgres")]
        public IActionResult UserProgres() 
        {
            //var user = _servise.userManager(). //userManager.Users.FirstOrDefault(u=>u.Email==User.Identity.Name.ToString());
           // user.NameImg = "sdvgsdfbsd";
            //userManager.
            //userManager.UpdateAsync(user);
            //userManager.UpdateNormalizedUserNameAsync(user);
            return View();
        }
        //
        [HttpGet]
        [Route("StatisticInfo")]
        public IActionResult StatisticInfo()
        {
            return View();
        }
        //
        [HttpGet]
        [Route("YourAccount")]
        public IActionResult YourAccount() 
        {
            var user = _servise.UserManager(User.Identity.Name.ToString());
            var words = _servise.GetAll(User.Identity.Name.ToString()).Count();
           ViewBag.Words = words;
            var rez = _servise.Rezults(User.Identity.Name.ToString());
            var countRez = rez.Count();
            ViewBag.Count = countRez;
            var point = rez.Sum(x => x.Gold);
            ViewBag.Point = point;
            return View(user);
        }
        //
        [HttpPost]
        [Route("UpdateImgUser")]
        public IActionResult UpdateImgUser(IFormFile file)
        {
            var user = _servise.UserManager(User.Identity.Name.ToString());
            if (file != null)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                user.Pict = ms.ToArray();
                ms.Close();
                ms.Dispose();
                _servise.UpdateUser(user);
               
            }
            return Redirect("YourAccount");
        }
        //
        [HttpGet]
        [Route("SetNameAvatar")]
        public IActionResult SetNameAvatar(string text) 
        {
            if (!String.IsNullOrEmpty(text)) 
            {
            var user = _servise.UserManager(User.Identity.Name.ToString());
            user.NameImg = text;
            _servise.UpdateUser(user);
            return Redirect("YourAccount");
            }
            return Redirect("YourAccount");
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
