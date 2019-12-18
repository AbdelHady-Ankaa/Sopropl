using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Sopropl_Backend.Controllers
{
    public class FallBackController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}