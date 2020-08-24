using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperUser.Models;
using SuperUser.ViewModels;

namespace Bugtracker.Controllers
{
    public class AccountController : Controller
    {
        //signin manager
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        //Inject into controller
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }


        [HttpGet]
        public ActionResult Login()
        {

            return View();

        }

        //Manually add users 
        public async Task<IActionResult> Register()
        {
            var user = new ApplicationUser { UserName = "test@gmail.com", Email = "test@gmail.com", FirstName = "Kobe", LastName = "Bryant" };
            await userManager.CreateAsync(user, "Hello123!");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            

            if (ModelState.IsValid)
            {

                
                var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);

                if (result.Succeeded)
                {

                    return RedirectToAction("index", "DashBoard");
                }


                ModelState.AddModelError(string.Empty, "Invalid Credentials");

            }



            return View(login);
        }


        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }

            else
            {
                return Json($"Email {email} is already in use ");
            }
        }
    }
}
