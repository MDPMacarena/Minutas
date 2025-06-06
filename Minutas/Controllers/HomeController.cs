using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MinutasManage.Models;
using System.Security.Claims;

namespace MinutasManage.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

       

        public IActionResult Index()
        {

            return RedirectToAction("Login", "Account");

        }


    }
}
