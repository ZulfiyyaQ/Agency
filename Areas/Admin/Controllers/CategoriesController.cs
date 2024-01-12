using Agency.Areas.Admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories= _context.Categories.Include(c=>c.Products).ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();
            bool result = _context.Categories.Any(c => c.Name.ToLower().Trim() == categoryVM.Name.ToLower().Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Bele Category artiq movcutdur");
                return View();
            }
            Category category = new Category
            {
                Name=categoryVM.Name 
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int id)
        {
            Category existed= await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            UpdateCategoryVM vm = new UpdateCategoryVM 
            { 
                Name= existed.Name
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();
            Category existed= await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (existed == null) return NotFound();
            bool result = _context.Categories.Any(c=>c.Name.ToLower().Trim()==categoryVM.Name.ToLower().Trim() && c.Id != id);
            if (result) 
            {
                ModelState.AddModelError("Name", "Bele Category artiq movcutdur");
                return View();
            }
            existed.Name=categoryVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id<=0) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(x => x.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }
    }
}
