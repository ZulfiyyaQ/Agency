using Agency.DAL;
using Agency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            List<Product> product = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(product);
        }
    }
}
