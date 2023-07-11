using EnglishWebSimulatorApp.Models.Interfaces;
using EnglishWebSimulatorApp.Models.Repository;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EnglishWebSimulatorApp.Models.WorkJson
{
    public class LibraryWorkJson : ILibraryWorkJson
    {
        public ILybraryWorkJsonRepository workJsonRepository { get; set; }

        private const string FilePath = "words.json";

        public LibraryWorkJson() 
        {
            var words_tmp = new List<LibraryWordsJson>();
            
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                words_tmp = JsonSerializer.Deserialize<List<LibraryWordsJson>>(json);
            }
            workJsonRepository = new LibraryWorkRepository (words_tmp);
        }
        public void SaveChange()
        {
            throw new System.NotImplementedException();
        }
    }
}
