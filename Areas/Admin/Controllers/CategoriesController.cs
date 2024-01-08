using Microsoft.AspNetCore.Mvc;

namespace Agency.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
