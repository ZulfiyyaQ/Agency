using Agency.Areas.Admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agency.Utilities.Extensions;

namespace Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            CreateProductVM productvm = new();
            GetList(productvm);
            return View(productvm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid) return View();
            bool result = _context.Products.Any(p => p.Name.ToLower().Trim() == productVM.Name.ToLower().Trim());
            if (result)
            {
                GetList(productVM);
                ModelState.AddModelError("Name", "Bele Product artiq movcutdur");
                return View();

            }
            if (!productVM.Photo.ValidateType())
            {
                GetList(productVM);
                ModelState.AddModelError("Photo", "Sekil File secmeyiniz mutleqdir");
                return View();
            }
            if (!productVM.Photo.ValidateSize(2 * 1024))
            {
                GetList(productVM);
                ModelState.AddModelError("Photo", "Sekil olcusu 2 mb dan cox olmamalidir");
                return View();
            }

            string filename = await productVM.Photo.CreateFile(_env.WebRootPath, "assets", "img", "portfolio");
            Product product = new Product
            {
                Image = filename,
                Name = productVM.Name,
                Description = productVM.Description,
                CategoryId = (int)productVM.CategoryId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Product existed = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            UpdateProductVM productVM = new UpdateProductVM
            {
                Name = existed.Name,
                Description = existed.Description,
                ImageUrl = existed.Image,
                CategoryId=existed.CategoryId,
                Categories=await _context.Categories.ToListAsync()
            };
            return View(productVM);  
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM productVM,int id)
        {
            Product existed = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if(!ModelState.IsValid)
            {
                GetList(productVM);
                return View(productVM);
            }
            if (existed is null) return NotFound();

            bool result = _context.Products.Any(p=>p.Name.ToLower().Trim()==productVM.Name.ToLower()&&p.Id!=id);
            if (result)
            {
                GetList(productVM);
                ModelState.AddModelError("Name", "Bele Product artiq movcutdur");
                return View(productVM);
            }

            bool result1 = _context.Categories.Any(p => p.Id == productVM.CategoryId);
            if (!result1)
            {
                GetList(productVM);
                ModelState.AddModelError("CategoryId", "Bele Category movcut deyildir");
                return View(productVM);
            }

            if(productVM.Photo is not null)
            {
                if (!productVM.Photo.ValidateType())
                {
                    GetList(productVM);
                    ModelState.AddModelError("Photo", "Sekil File secmeyiniz mutleqdir");
                    return View();
                }
                if (!productVM.Photo.ValidateSize(2 * 1024))
                {
                    GetList(productVM);
                    ModelState.AddModelError("Photo", "Sekil olcusu 2 mb dan cox olmamalidir");
                    return View();
                }

                string newimage = await productVM.Photo.CreateFile(_env.WebRootPath, "assets", "img", "portfolio");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "portfolio");
                existed.Image = newimage;
            }
            existed.Name=productVM.Name;
            existed.Description=productVM.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Product existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) return NotFound();
            _context.Products.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product is null) return NotFound();
            return View(product);
        }

        private void GetList(CreateProductVM vm)
        {
            vm.Categories = _context.Categories.ToList();

        }
        private void GetList(UpdateProductVM vm)
        {
            vm.Categories = _context.Categories.ToList();

        }
    }
}
