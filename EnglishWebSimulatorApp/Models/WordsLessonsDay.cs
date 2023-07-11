using System;
using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models
{
    public class WordsLessonsDay
    {
        public DateTime date { get; set; }
        public List<string> words { get; set; }
        public List<string> wordsJson { get; set; }
    }
}
