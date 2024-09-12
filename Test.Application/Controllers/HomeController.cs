using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test.Application.Models;

namespace Test.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationContext context, IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_context.Files.ToList());
        }

        // Извлекает по 1 фото из папки
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/files/"+uploadedFile.FileName;

                using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath+path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                FileModel file = new FileModel
                {
                    Name=uploadedFile.FileName,
                    Path=path
                };

                _context.Files.Add(file);
                _context.SaveChanges();

            }

            return RedirectToAction("Index");
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
