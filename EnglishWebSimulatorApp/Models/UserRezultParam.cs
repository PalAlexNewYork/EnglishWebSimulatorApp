using System.Threading;

namespace EnglishWebSimulatorApp.Models
{
    public class UserRezultParam
    {
        public string User { get; set; }
        public string Email { get; set; }
        public byte[] Pict { get; set; }
        public int NumberOfWords { get; set; }
        public int Point { get; set; }
        public int NumberOfLesson { get; set;}
    }
}
