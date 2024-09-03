using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

using onl.Data;
using onl.Models;


namespace onl.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext db;
        public AuthController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult SignUp(User u)
        {
            var existing = db.UserInfo.SingleOrDefault(x => x.Username == u.Username);

            if (existing != null)
            {
                TempData["UsernameExists"] = "Username already exists. Please choose a different one.";
                return View(u);
            }

            u.Role = "User";
            db.UserInfo.Add(u);
            db.SaveChanges();
            return RedirectToAction("SignIn");
        }

        public IActionResult SignIn()
        {
            return View();
            //rohith
        }
        [HttpPost]
        public IActionResult SignIn(SignIn log)
        {
            var data = db.UserInfo.Where(x => x.Username.Equals(log.Username)).SingleOrDefault();
            if (data != null)
            {
                bool us = data.Username.Equals(log.Username) && data.Password.Equals(log.Password);
                if (us)
                {
                    HttpContext.Session.SetString("role", data.Role);

                    HttpContext.Session.SetString("username",data.Username);
                    HttpContext.Session.SetString("userid", data.Id.ToString());


                    return RedirectToAction("Index", "Test");
                }
                else
                {
                    TempData["IncorrectEmail"] = "IncorrectEmail";
                }
            }
            else
            {
                TempData["IncorrectPassword"] = "Incorrect Password";
            }
            return View();
        }
    }
}

