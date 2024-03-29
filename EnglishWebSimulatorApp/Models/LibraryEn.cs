﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EnglishWebSimulatorApp.Models
{
    public class LibraryEn
    {
        public int Id { get; set; }
        [Required]
        public string WordEng { get; set; }
        [Required]
        public string WordRus { get; set; }
        public string UserName { get; set; }
        public string Pict { get; set; }
        public string SoundFilePath { get; set; }
        public DateTime DateTime { get; set; }
        public  string Thema { get; set; }
    }
}
