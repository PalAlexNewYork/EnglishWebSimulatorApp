using System.Collections.Generic;

namespace EnglishWebSimulatorApp.Models.Interfaces
{
    public interface ILibraryWorkJson
    {
        public  ILybraryWorkJsonRepository workJsonRepository { get; set; }
        public void SaveChange();
    }
}
