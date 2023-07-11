using System.IO;

namespace EnglishWebSimulatorApp.Models
{
    public class LibraryWordsJson
    {
        public  int Id { get; set; }
        public string En { get; set; }
        public string Ru { get; set; }
        public string Tr { get; set; }
        public string PathPict { get; set; }
        public string PathSound { get; set; }
    }
}
