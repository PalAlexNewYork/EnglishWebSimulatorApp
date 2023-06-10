using EnglishWebSimulatorApp.Areas.Identity.Data;
using EnglishWebSimulatorApp.Data;
using EnglishWebSimulatorApp.Models.Interfaces;
using EnglishWebSimulatorApp.Models.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public List<LibraryEn> GetAll()=>this.context.libraryEns.ToList();
       
        public List<LibraryEn> GetAll(string user)
        {
            var words =  this.context.libraryEns.Where(w=>w.User == user).ToList();
            return words;
        }

        public void Update(LibraryEn en, string us)
        {
            var w= this.context.libraryEns.FirstOrDefault(w => w.Id == en.Id);
            w.WordRus=en.WordRus; 
            w.WordEng = en.WordEng;
            w.SoundFilePath=en.SoundFilePath;
            w.Pict=en.Pict;
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
            var rezults = this.context.Rezults.Where(r=>r.User == user).ToList();
            return rezults;
        }

        public void AddRezult(Rezults rezults)
        {
            this.context.Entry(rezults).State = EntityState.Added;
            this.context.SaveChanges();
        }

        public EnglishWebSimulatorAppUser UserManager(string user)
        {
            return  _userManager.Users.FirstOrDefault(u=>u.Email == user);
        }

        public EnglishWebSimulatorAppUser UpdateUser(EnglishWebSimulatorAppUser user)
        { 
        _userManager.UpdateNormalizedUserNameAsync(user);
            this.context.SaveChanges();
            return user;
        }
    }
}
