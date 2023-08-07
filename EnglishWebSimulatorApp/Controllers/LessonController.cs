using EnglishWebSimulatorApp.Models;
using EnglishWebSimulatorApp.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult StartLesson(string param)
        {
           ViewBag.StartLesson = param;
           var selects =  _servise.SetSelectDateTheme(null, User.Identity.Name.ToString(), null); 
           ViewBag.Select = selects;
            return View();
        }
        //
        [HttpGet]
        [Route("AddBook")]
        public IActionResult AddBook(string check, int number, string radio, string param, string text)
        {
            List<LibraryEnShow> libraries = new List<LibraryEnShow>();
            if (check == "CheckWordsListUser")
            {
                var wordsUser = _servise.GetAll(User.Identity.Name.ToString());
                List<MyTuple> wordsStr = new List<MyTuple>();
                foreach (var i in wordsUser) wordsStr.Add(new MyTuple(i.WordEng, i.WordRus));
                if (param == "ear") ViewBag.lesson = "ear";
                    return View("CheckList", wordsStr);
            }
            else
            {
                libraries = _servise.ChoiceOfWords(check, number, radio, User.Identity.Name.ToString(), text);
                if (libraries != null) 
                {
                libraryEnShService.DeleteAllWords();
                foreach (var lib in libraries)
                {
                    libraryEnShService.AddWordLibEnSh(lib);
                    libraryEnShService.AddIdWord(lib.Id);
                }
                ViewBag.right = libraryEnShService.rightAnswer;
                ViewBag.notRight = libraryEnShService.notRightAnswer;
                    if (param == "ear")
                    {
                        ViewBag.lesson = "ear";
                          return View("Lesson", libraries);
                    }
                    else if (param == "wordEng")
                    {
                        ViewBag.lesson = "WEng";
                        return View("LessonWordEng", libraries);
                    }
                    else return View("Lesson", libraries);

                }
                else
                    return View("StartLesson");
            }
        }
        //
        [HttpPost]
        [Route("LessonFormList")]
        public IActionResult LessonFormList(IEnumerable<string> groups, string lesson)
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
            if (lesson == "ear") ViewBag.lesson = lesson;
                return View("Lesson", libraries);
        }

        [HttpPost]
        [Route("Check")]
        public IActionResult Check(string word, string wordRu, string lesson)
        {
            var libraries = libraryEnShService.GetAll();
            var wordObj = libraries.FirstOrDefault(w => w.WordRus == wordRu);
            bool isError = false;
            if (_servise.CompareWords(word, wordObj, lesson))
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
            {
                if (lesson == "ear") {
                    ViewBag.lesson = "ear";
                    return View("Lesson", libraries_redirect); 
                }
                else if(lesson == "WEng")
                {
                    ViewBag.lesson = "WEng";
                    return View("LessonWordEng", libraries_redirect);
                }
                else return View("Lesson", libraries_redirect);
            } 
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
            rezults.IdWords = libraryEnShService.IdWords;
            rezults.Id = 0; rezults.IsIdWordsDatabase = true;
            if (ModelState.IsValid) 
            {
                _servise.AddRezult(rezults);
            }
            var selects = _servise.SetSelectDateTheme(null, User.Identity.Name.ToString(), null);
            ViewBag.Select = selects;
            return View("StartLesson");
        }
        //
        [HttpGet]
        [Route("UserProgres")]
        public IActionResult UserProgres() 
        {
            var user = _servise.UserManager(User.Identity.Name.ToString());
            if(user.NameImg ==null)
            ViewBag.User = User.Identity.Name;
            else ViewBag.User = user.NameImg;
            string pict = "";
            if (user.Pict != null) 
            { 
            string imageBase64Data = Convert.ToBase64String(user.Pict);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            pict = imageDataURL;
            ViewBag.Pict = pict;
            }
            var rezults = _servise.GetRezultsWeek(User.Identity.Name.ToString());        
            return View(rezults);
        }
        //
        [HttpGet]
        [Route("StatisticInfo")]
        public IActionResult StatisticInfo()
        {
            var userRezult = _servise.GetUserRezult();
            userRezult = userRezult.OrderByDescending(u=>u.Point).ToList();
            // находим место в рейтинге юзера
            var u = userRezult.FirstOrDefault(u=>u.Email == User.Identity.Name.ToString());
            int place = userRezult.IndexOf(u);
            ViewBag.Place = place+1;
            return View(userRezult);
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
//
        public IActionResult Index()
        {
            return View();
        }
    }
}
