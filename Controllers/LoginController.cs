using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using csharp_belt.Models;
using System.Linq;

namespace csharp_belt.Controllers
{
    public class LoginController : Controller
    {
        private BeltContext _context;
 
        public LoginController(BeltContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel user)
        {
            if (ModelState.IsValid) {
                User newuser = new User {
                    FirstName = user.FirstName,
                    LastName= user.LastName,
                    Username = user.Username,
                    Password = user.Password,
                    CreatedAt = DateTime.Now,
                    UploadedAt = DateTime.Now,
                };
                User getuser = _context.users.SingleOrDefault(get => get.Username == user.Username);
                if (getuser == null) {
                    _context.users.Add(newuser);
                    Wallet newwallet = new Wallet {
                        UserId = newuser.UserId,
                        Amount = 1000.00,
                        CreatedAt = DateTime.Now,
                        UploadedAt = DateTime.Now,
                    };
                    _context.wallets.Add(newwallet);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("userid", newuser.UserId);
                    return RedirectToAction("Index", "Auction");
                } else {
                    ViewBag.Error = "Username already in database.";
                }
            }
            return View("Index");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            ViewBag.Error = "false";
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Username, string Password)
        {
            User getuser = _context.users.SingleOrDefault(get => get.Username == Username);
            if (getuser != null && Password != null) {
                if (getuser.Password == Password) {
                    HttpContext.Session.SetInt32("userid", getuser.UserId);
                    return RedirectToAction("Index", "Auction");
                }
                ViewBag.Error = "Username/Password Invalid.";
            } else {
                ViewBag.Error = "Username/Password Invalid. May need to register.";
            }
            return View("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }
    }
}