using Bilet14.DAL;
using Bilet14.Models;
using FinalTemplate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bilet14.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
               
                OurServices = _context.OurServices.ToList(),
                
            };

            return View(homeVM);
        }

        
    }
}