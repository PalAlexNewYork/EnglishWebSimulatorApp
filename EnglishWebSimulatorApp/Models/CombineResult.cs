using System;
using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models
{
    public class CombineResult
    {
        public DateTime currentDay {  get; set; }
        public List<Rezults> rezults { get; set; }
        public List<LibraryEn> libraryEns { get; set; }
        public int Days { get; set; } 
        public List<WordsLessonsDay> wordsLesson { get; set; }

    }
}
