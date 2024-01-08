using Microsoft.AspNetCore.Mvc;

namespace Agency.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
