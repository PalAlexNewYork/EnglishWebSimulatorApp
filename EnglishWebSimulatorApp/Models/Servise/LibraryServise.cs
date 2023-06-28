using EnglishWebSimulatorApp.Areas.Identity.Data;
using EnglishWebSimulatorApp.Data;
using EnglishWebSimulatorApp.Models.Interfaces;
using EnglishWebSimulatorApp.Models.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishWebSimulatorApp.Models.Servise
{
    public class LibraryServise : ILibraryServise
    {
        private readonly EnglishWebSimulatorAppContext context;
        private ILibratyRepository libratyRepository;
        private IRezultsRepository rezultsRepository;
        UserManager<EnglishWebSimulatorAppUser> _userManager;
        public LibraryServise(EnglishWebSimulatorAppContext context, UserManager<EnglishWebSimulatorAppUser> userManager)
        {
            this.context = context;
            var words = this.context.libraryEns.ToList();
            var rezults = this.context.Rezults.ToList();
            rezultsRepository = new RezultsRepository(rezults);
            libratyRepository = new LibraryEnRepository(words);
            _userManager = userManager;
        }

        public void AddWord(LibraryEn en)
        {
            this.context.Entry(en).State = EntityState.Added;
            context.SaveChanges();
        }

        public List<LibraryEn> GetAll() => this.context.libraryEns.ToList();

        public List<LibraryEn> GetAll(string user)
        {
            var words = this.context.libraryEns.Where(w => w.User == user).ToList();
            return words;
        }

        public void Update(LibraryEn en, string us)
        {
            var w = this.context.libraryEns.FirstOrDefault(w => w.Id == en.Id);
            w.WordRus = en.WordRus;
            w.WordEng = en.WordEng;
            w.SoundFilePath = en.SoundFilePath;
            w.Pict = en.Pict;
            this.context.Entry(w).State = EntityState.Modified;
            context.SaveChanges();
        }

        public List<LibraryEnShow> librariesShow(List<LibraryEn> libraries)
        {
            List<LibraryEnShow> list = new List<LibraryEnShow>();
            foreach (var word in libraries)
            {
                LibraryEnShow tmp = new LibraryEnShow();
                tmp.Id = word.Id;
                tmp.WordEng = word.WordEng;
                tmp.WordRus = word.WordRus;
                //
                if (word.Pict != null)
                {
                    string imageBase64Data = Convert.ToBase64String(word.Pict);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    tmp.Pict = imageDataURL;
                }
                if (word.SoundFilePath != null)
                    tmp.Sound = word.SoundFilePath;
                    tmp.Thema = word.Thema;
                list.Add(tmp);
            }
            return list;
        }

        public void Remove(int id)
        {
            var w = this.context.libraryEns.FirstOrDefault(w => w.Id == id);
            this.context.Remove(w);
            this.context.SaveChanges();
        }

        public List<Rezults> Rezults(string user)
        {
            var rezults = this.context.Rezults.Where(r => r.User == user).ToList();
            return rezults;
        }

        public void AddRezult(Rezults rezults)
        {
            this.context.Entry(rezults).State = EntityState.Added;
            this.context.SaveChanges();
        }

        public EnglishWebSimulatorAppUser UserManager(string user)
        {
            return _userManager.Users.FirstOrDefault(u => u.Email == user);
        }

        public List<EnglishWebSimulatorAppUser> GetUsers() => _userManager.Users.ToList();

        public EnglishWebSimulatorAppUser UpdateUser(EnglishWebSimulatorAppUser user)
        {
            _userManager.UpdateNormalizedUserNameAsync(user);
            this.context.SaveChanges();
            return user;
        }

        public CombineResult GetRezultsWeek(string user)
        {
            CombineResult rezultDay = new CombineResult();
            var rezult = rezultsRepository.GetRezults(user);
            List<Rezults> rezults = new List<Rezults>();
            DateTime date1 = DateTime.Now;
            DateTime date2 = date1.AddDays(-6);
            //добавляем текущий день
            rezultDay.currentDay = date1;
            for (DateTime x = date1; x >= date2; x = x.AddDays(-1))
            {
                foreach (Rezults item in rezult)
                    if (x.Day == item.Data.Day && x.Month == item.Data.Month && x.Year == item.Data.Year) rezults.Add(item);
            }
            //добавляем результаты за неделю
            rezultDay.rezults = rezults;
            //Считаем слова добавленные за неделю
            List<LibraryEn> libraries = new List<LibraryEn>();
            var words = libratyRepository.GetAll().Where(w => w.User == user).ToList();
            for (DateTime x = date1; x >= date2; x = x.AddDays(-1))
            {
                foreach (LibraryEn item in words)
                    if (x.Day == item.DateTime.Day && x.Month == item.DateTime.Month && x.Year == item.DateTime.Year) libraries.Add(item);
            }
            rezultDay.libraryEns = libraries;
            //Считаем сколько дней непрерывной работы
            int countDay = 0;
            for (DateTime x = date1; x >= date2; x = x.AddDays(-1))
            {
                bool flg = false;
                foreach (Rezults item in rezult)
                {
                    if (x.Day == item.Data.Day && x.Month == item.Data.Month && x.Year == item.Data.Year)
                    {
                        flg = true; break;
                    }
                }
                if (!flg) break;
                foreach (LibraryEn item in words)
                    if (x.Day == item.DateTime.Day && x.Month == item.DateTime.Month && x.Year == item.DateTime.Year)
                    {
                        flg = true; break;
                    }
                if (flg)
                {
                    countDay++;
                }
            }
            rezultDay.Days = countDay;
            //Считаем слова пройденные за неделю
            List<WordsLessonsDay> wordsLessons = new List<WordsLessonsDay>();

            for (DateTime x = date1; x >= date2; x = x.AddDays(-1))
            {
                WordsLessonsDay tmp = new WordsLessonsDay();
                tmp.date = x;
                List<string> tmpStr = new List<string>();
                string tmp_string = "";
                foreach (var i in rezults)
                {
                    if (x.Day == i.Data.Day) tmp_string += i.IdWords;
                }
                if (tmp_string != "")
                {
                    var strings = tmp_string.Split('%').ToList();
                    strings.Remove("");
                    List<int> stringsId = strings.Select(i => Int32.Parse(i)).ToList();
                    var IdWord = new HashSet<int>(stringsId).ToList();
                    foreach (int id in IdWord)
                    {
                        foreach (LibraryEn i in words)
                        {
                            if (id == i.Id)
                                tmpStr.Add(i.WordEng);
                        }
                    }
                    tmp.words = tmpStr;
                    wordsLessons.Add(tmp);
                }
            }
            rezultDay.wordsLesson = wordsLessons;
            return rezultDay;
        }

        public List<LibraryEnShow> ChoiceOfWords(string check, int number, string radio, string user)
        {
            var l = libratyRepository.GetAll().Where(w => w.User == user).ToList();
            var libraries = this.librariesShow(l);

            if (check == "NumberWords")//колличество слов от 5 до 50, можно последних или рандомных из всего списка
            {
                var wordsTmp = new List<LibraryEnShow>(); var rnd = new Random();
                if (libraries.Count > number)
                {
                    if (radio == "random")
                        wordsTmp = libraries.OrderBy(x => rnd.Next()).Take(number).ToList();
                    else
                        wordsTmp = libraries.Cast<LibraryEnShow>().Reverse().Take(number).ToList();
                    libraries = wordsTmp.ToList();
                }
                else
                {
                    var word10 = libraries.Cast<LibraryEnShow>().Reverse().Take(libraries.Count).ToList();
                    libraries = word10.ToList();
                }
            }
            else if (check == "tenWords") //последние 10 слов
            {
                if (libraries.Count > 10)
                {
                    var word10 = libraries.Cast<LibraryEnShow>().Reverse().Take(10).ToList();
                    libraries = word10.ToList();
                }
            }
            else if (check == "fiveWords")//последние 5 слов
            {
                if (libraries.Count > 5)
                {
                    var word5 = libraries.Cast<LibraryEnShow>().Reverse().Take(5).ToList();
                    libraries = word5.ToList();
                }
            }
            else if (check == "GetWordsAllUsers")//cлова всех юзеров
            {
                var librariesGetUsers = libratyRepository.GetAll();
                libraries = this.librariesShow(librariesGetUsers);
                var Words = new HashSet<LibraryEnShow>(libraries).ToList();
                libraries = Words.ToList();
            }
            else if (check == "GetWordsUser")
                return libraries;
            else
                libraries = null;
            return libraries;
        }

        public List<UserRezultParam> GetUserRezult()
        {
            var users = _userManager.Users.ToList();
            var words = libratyRepository.GetAll().ToList();
            List<UserRezultParam> UsersRezult = new List<UserRezultParam>();
            foreach (var u in users)
            {
                UserRezultParam tmp = new UserRezultParam();
                tmp.User = u.NameImg;
                tmp.Email = u.Email;
                tmp.Pict = u.Pict;
                int numOfWords = words.Where(w => w.User == u.Email).ToList().Count();
                int numOfLes = this.rezultsRepository.GetRezults(u.Email).ToList().Count();
                int tmpPoint = this.rezultsRepository.GetRezults(u.Email).Sum(r => r.Gold);
                tmp.NumberOfWords = numOfWords;
                tmp.NumberOfLesson = numOfLes;
                tmp.Point = tmpPoint;
                UsersRezult.Add(tmp);
            }
            return UsersRezult;
        }

        public bool CompareWords(string word, LibraryEnShow wordObj, string lesson)
        {
            bool isWords = false;
            if (lesson == "WEng")
            {
                if (wordObj.WordRus.Equals(word, StringComparison.OrdinalIgnoreCase))
                {
                    isWords = true;
                }
                if (isWords) return true;
                else
                {

                    var s = new string(wordObj.WordRus.Where(c => !char.IsPunctuation(c)).ToArray());
                    var wordsRu = s.Split(" ").ToList();
                    if (wordsRu.Any(e => e.Equals(word, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            else 
            {
                if (wordObj.WordEng.Equals(word, System.StringComparison.OrdinalIgnoreCase))
                    return true;
                else
                    return false;
            }
        }

        public LibraryEn GetWordsId(int id, string user) =>libratyRepository.GetAll().Where(w => w.User == user).ToList().FirstOrDefault(w => w.Id == id);

        public Tuple<int, List<string>> SetSelectDateTheme(string theme, string user)
        {
            var themas_tmp = libratyRepository.GetAll().Where(w => w.User == user).ToList().Select(x => x.Thema);
            List<string> themas = new HashSet<string>(themas_tmp).ToList();
            int ind = 0;
            if (theme != null) foreach (var i in themas) { if (i == theme) break; ++ind; }
            return Tuple.Create(ind, themas);
        }
    }
}
