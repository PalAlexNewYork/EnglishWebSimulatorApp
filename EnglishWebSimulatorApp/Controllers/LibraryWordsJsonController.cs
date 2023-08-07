using EnglishWebSimulatorApp.Models;
using EnglishWebSimulatorApp.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishWebSimulatorApp.Controllers
{
    public class LibraryWordsJsonController : Controller
    {
        private readonly ILibraryWorkJsonServise library;
        private readonly IlibraryEnShServise libraryEnShService;
        private readonly ILibraryServise libraryServise;
       
        public LibraryWordsJsonController(ILibraryWorkJsonServise library, IlibraryEnShServise libraryEnShService, ILibraryServise libraryServise)
        {
            this.library = library;
            this.libraryEnShService = libraryEnShService;
            this.libraryServise = libraryServise;
        }

        public IActionResult MainLessonLibrary(string namePage) 
        { 
            ViewBag.CountWord = this.library.GetAllWords().Count;
            ViewBag.Name = namePage;
            return View();
        }
        //
        [HttpPost]
        [Route("SelectWords")]
        public IActionResult SelectWords(string check, int number, int numberFrom, int numberTo, string str, string namePage)
        {
            List<LibraryWordsJson> list = new List<LibraryWordsJson>();
            ViewBag.Name = namePage;
            if (check != "HeadSelectWords") 
            {
                list = this.library.SelectWordsServise(check, number, str, numberFrom, numberTo);
                if (list.Count != 0) 
                {
                    libraryEnShService.DeleteAllWords();
                    libraryEnShService.AddAllWordsJson(list);
                    ViewBag.right = libraryEnShService.rightAnswer;
                    ViewBag.notRight = libraryEnShService.notRightAnswer;
                    ViewBag.flag = true; 
                    return View("MainLessonLibrary", list);
                }
                else return View("MainLessonLibrary");
            }        
            else 
            {
                list = this.library.GetAllWords(); 
                if (list.Count != 0) return View("PreselectionWords", list);
                else return View("MainLessonLibrary");              
            }
        }
        //
        [HttpPost]
        [Route("PreselectionWordsEnd")]
        public IActionResult PreselectionWordsEnd(IEnumerable<string> groups, string namePage) 
        {
            List<LibraryWordsJson> list = new List<LibraryWordsJson>();
            if (groups.Count() != 0)  list = this.library.GetWordsHand(groups);
            if (list.Count != 0)
            {
                libraryEnShService.DeleteAllWords();
                libraryEnShService.AddAllWordsJson(list);
                ViewBag.right = libraryEnShService.rightAnswer;
                ViewBag.notRight = libraryEnShService.notRightAnswer;
                ViewBag.flag = true; ViewBag.Name = namePage;
                return View("MainLessonLibrary", list);
            }
            else
            {
                return View("MainLessonLibrary");
            }    
        }
        //
        [HttpPost]
        [Route("Check")]
        public IActionResult Check(string word, string wordRight, string namePage) 
        {
            if (this.libraryEnShService.GetAllWordsJson().Count == 0)
            {
                if (namePage == "knowledgeTesting")return Redirect("AddRezult");
                else
                {
                    ViewBag.stepp = libraryEnShService.rightAnswer + libraryEnShService.notRightAnswer;
                    if (libraryEnShService.notRightAnswer != 0) ViewBag.danat = libraryEnShService.rightAnswer / libraryEnShService.notRightAnswer;
                    else ViewBag.danat = libraryEnShService.rightAnswer;
                    if (libraryEnShService.notRightAnswer != 0) ViewBag.precentage = (libraryEnShService.notRightAnswer / libraryEnShService.rightAnswer) * 100;         //(100 - ((ViewBag.notRight * 100) / ViewBag.right));
                    else ViewBag.precentage = 100;
                    return View("EndLesson");
                }
            }

            if (wordRight == null)
            {
                var _list = this.libraryEnShService.GetAllWordsJson();
                ViewBag.right = libraryEnShService.rightAnswer;
                ViewBag.NotRight = libraryEnShService.notRightAnswer;
                ViewBag.Name = namePage; ViewBag.flag = true;
                return View("MainLessonLibrary", _list);
            }
            else 
            {
                if (library.ComparedWords(word, wordRight))
                {
                    this.libraryEnShService.rightAnswer += 1;
                    libraryEnShService.DeleteWord(this.libraryEnShService.GetWords(wordRight));
                    ViewBag.isError = true;
                }
                else
                {
                    this.libraryEnShService.notRightAnswer += 1;
                    ViewBag.isError = false;
                    if (namePage == "knowledgeTesting") 
                    {
                        var wordR = this.libraryEnShService.GetWords(wordRight);
                        libraryEnShService.DeleteWord(wordR);
                        ViewBag.En = wordR.En; ViewBag.Ru = wordR.Ru; ViewBag.Tr = wordR.Tr; 
                        if(wordR.PathPict!=null) ViewBag.Img = wordR.PathPict;
                        if(wordR.PathSound!=null) ViewBag.Sound = wordR.PathSound;
                        ViewBag.right = libraryEnShService.rightAnswer;
                        ViewBag.NotRight = libraryEnShService.notRightAnswer;
                        ViewBag.Name = namePage; ViewBag.flag = true;
                        var _list = this.libraryEnShService.GetAllWordsJson();
                        return View("MainLessonLibrary", _list);
                    }
                }
            }
            if (this.libraryEnShService.GetAllWordsJson().Count == 0)
            {
                if (namePage == "knowledgeTesting") return Redirect("AddRezult");
                else
                {
                    ViewBag.right = libraryEnShService.rightAnswer;
                    ViewBag.NotRight = libraryEnShService.notRightAnswer;
                    ViewBag.stepp = libraryEnShService.rightAnswer + libraryEnShService.notRightAnswer;
                    if (libraryEnShService.notRightAnswer != 0) ViewBag.danat = libraryEnShService.rightAnswer / libraryEnShService.notRightAnswer;
                    else ViewBag.danat = libraryEnShService.rightAnswer;
                    if (libraryEnShService.notRightAnswer != 0)
                        ViewBag.precentage = Math.Round((Math.Round((
                            Convert.ToDecimal(libraryEnShService.rightAnswer- libraryEnShService.notRightAnswer) / Convert.ToDecimal(libraryEnShService.rightAnswer)), 2) * 100), 0);         //(100 - ((ViewBag.notRight * 100) / ViewBag.right));
                    else ViewBag.precentage = 100;
                    return View("EndLesson");
                }
            }
            var list = this.libraryEnShService.GetAllWordsJson();
            ViewBag.right = libraryEnShService.rightAnswer;
            ViewBag.NotRight = libraryEnShService.notRightAnswer;
            ViewBag.Name = namePage; ViewBag.flag = true;
            return View("MainLessonLibrary", list);
        }
        //
        [HttpGet]
        [Route("AddRezult")]
        public IActionResult AddRezult()
        {
            Rezults rezults = new Rezults();
            rezults.Words = libraryEnShService.rightAnswer+ libraryEnShService.notRightAnswer;
            rezults.Error = libraryEnShService.notRightAnswer;
            rezults.User = User.Identity.Name.ToString();
            rezults.Data = DateTime.Now;
            if (libraryEnShService.notRightAnswer != 0) rezults.Gold = libraryEnShService.rightAnswer / libraryEnShService.notRightAnswer;
            else rezults.Gold = libraryEnShService.rightAnswer;
            rezults.IdWords = libraryEnShService.IdWords;
            rezults.Id = 0;
            rezults.IsIdWordsDatabase = false;
            if (ModelState.IsValid) this.libraryServise.AddRezult(rezults);
            if (rezults.Error != 0) ViewBag.danat = rezults.Words / rezults.Error;
            else ViewBag.danat = rezults.Words;
            ViewBag.rightAnsw = rezults.Words - rezults.Error;
            if (rezults.Error != 0) ViewBag.precentage = Math.Round((Math.Round((Convert.ToDecimal(rezults.Words - rezults.Error) / Convert.ToDecimal(rezults.Words)), 2) * 100), 0);         //(100 - ((rezults.Error * 100) / (rezults.Words - rezults.Error)));
            else ViewBag.precentage = 100;
            ViewBag.Words = library.GetWordsEnStr(libraryEnShService.IdWords);
            return View("EndLesson", rezults);
        }
            public IActionResult IndexTmp() 
        {
            return View();
        }
    }
}
