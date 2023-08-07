using EnglishWebSimulatorApp.Models;
using EnglishWebSimulatorApp.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EnglishWebSimulatorApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ILibraryServise _servise;

        public HomeController(ILogger<HomeController> logger, ILibraryServise servise)
        {
            _logger = logger;
            _servise = servise;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
        //
        [HttpGet]
        [Route("AboutApp")]
        public IActionResult AboutApp() 
        {
            return View();
        }
        //
        [HttpGet]
        [Route("AddWords")]
        public IActionResult AddWords(int id, string name) 
        {
            if (id != 0)
            {
                var word = _servise.GetWordsId(id, User.Identity.Name.ToString());
                ViewBag.SelectList = _servise.SetSelectDateTheme(word.Thema, User.Identity.Name.ToString(), name);
                ViewBag.PathImg = word.Pict;
                return View("AddWords", word);
            }
            else 
            {
                ViewBag.SelectList = _servise.SetSelectDateTheme(null, User.Identity.Name.ToString(), name);
                return View("AddWords");
            }
        }
        //
        [HttpPost]
        [Route("AddWordsMethod")]
        public async Task<IActionResult> AddWordsMethod(LibraryEn word, IFormFile file, IFormFile fileOne) 
        {
            int word_id = 0;
            word.User = User.Identity.Name.ToString();
            word.DateTime = DateTime.Now;
            if (word.Id == 0)
            {
                if (file != null)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    word.Pict = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
                if (fileOne != null && fileOne.Length != 0)
                {
                    var path = Path.Combine(
                                Directory.GetCurrentDirectory(), $"wwwroot/sounds/",
                                fileOne.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileOne.CopyToAsync(stream);
                    }
                    string NameFile = Path.GetFileName(fileOne.FileName);
                    word.SoundFilePath = NameFile;
                }
                if (ModelState.IsValid) 
                {
                    if (!(_servise.GetAll(User.Identity.Name.ToString()).All(w => w.WordEng.Equals(word.WordEng, StringComparison.OrdinalIgnoreCase))))
                        _servise.AddWord(word);
                    word_id = _servise.GetAll(User.Identity.Name.ToString()).FirstOrDefault(w => w.WordEng.Equals(word.WordEng, StringComparison.OrdinalIgnoreCase)).Id;
                }

            }
            else 
            {
                var wordObj = _servise.GetAll(User.Identity.Name.ToString()).FirstOrDefault(w => w.Id == word.Id);
                if (file != null)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    word.Pict = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
                else 
                {
                    word.Pict = wordObj.Pict;
                }
                if (fileOne != null && fileOne.Length != 0)
                {
                    var path = Path.Combine(
                                Directory.GetCurrentDirectory(), $"wwwroot/sounds/",
                                fileOne.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileOne.CopyToAsync(stream);
                    }
                    string NameFile = Path.GetFileName(fileOne.FileName);
                    word.SoundFilePath = NameFile;
                }
                else
                {
                    word.SoundFilePath = wordObj.SoundFilePath;
                }
                if (ModelState.IsValid) 
                {
                    _servise.Update(word, User.Identity.Name.ToString());
                    word_id = word.Id;
                }
            }
            return Redirect($"ShowWords?word_id={word_id}");
        }
        //
        [HttpGet]
        [Route("ShowWords")]
        public IActionResult ShowWords(int word_id) 
        {
            var words =   _servise.GetAll(User.Identity.Name.ToString());
            var list = _servise.librariesShow(words);
            var rnd = new Random();
            var randomized = list.OrderBy(item => rnd.Next()).ToList();
           if (word_id != 0) 
            {
                var index = randomized.FindIndex(x => x.Id == word_id);
                 var item = randomized[index];
                randomized[index] = randomized[0];
                randomized[0] = item;
            }
            ViewBag.SelectList = _servise.SetSelectDateTheme(null, User.Identity.Name.ToString(), null);
            return View("ShowWords", randomized);
        }
        //
        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            _servise.Remove(id);
            return Redirect("ShowWords");
        }
        //
        [HttpGet]
        [Route("FindWord")]
        public IActionResult FindWord(string text)
        {
            if (!string.IsNullOrEmpty(text)) 
            {
                string pattern = @"\p{IsCyrillic}";
                var words = _servise.GetWordsFragmentStr(User.Identity.Name, text, (Regex.Matches(text, pattern).Count > 0)); return View("ShowWords", words);
            }
            return Redirect("ShowWords");
        }
        //
        [HttpGet]
        [Route("FineWordsTheme")]
        public IActionResult FineWordsTheme (string text) 
        {
            var list = _servise.GetLibrariesShowThema(text, User.Identity.Name.ToString());
            ViewBag.Theme = list[0].Thema;
            return View("ShowWords", list);
        }
        //
        [HttpGet]
        [Route("ShowCardsWords")]
        public IActionResult ShowCardsWords() 
        {
            return View();
        }
    }
}
