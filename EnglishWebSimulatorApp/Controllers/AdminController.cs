using EnglishWebSimulatorApp.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWebSimulatorApp.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly ILibraryServise _libraryServise;

        public AdminController(ILibraryServise libraryServise)
        {
            _libraryServise = libraryServise;
        }

        public IActionResult Index()
        {
            var tmp = User.Identities.ToString();
            return View();
        }
    }
}
