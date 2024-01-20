using Agency.Areas.Admin.ViewModels;
using Agency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager <AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;  
            _signInManager = signInManager;
           _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM uservm)
        {
            if(!ModelState.IsValid) return View();
            AppUser user = new AppUser 
            {
                UserName = uservm.UserName,
                Name= uservm.Name,
                Email = uservm.Email,
                Surname=uservm.Surname
                
            };
             var result = await _userManager.CreateAsync(user,uservm.Password);
            if(!result.Succeeded) 
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, item.Description);
                } 
                return View();
            }

             await  _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM uservm)
        {
            if(!ModelState.IsValid) return View();
           AppUser user= await _userManager.FindByNameAsync(uservm.UserNameOrEmail);
            if (user==null)
            {
                user = await _userManager.FindByEmailAsync(uservm.UserNameOrEmail);
                if (user==null)
                {
                    ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect");
                    return View();
                }
            }
            var result=await _signInManager.PasswordSignInAsync(user,uservm.Password,uservm.IsRemembered,true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or Password is incorrect");
                return View();
            }



            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "admin"
            });
            return RedirectToAction("Index","Home");
        }
    }
}
