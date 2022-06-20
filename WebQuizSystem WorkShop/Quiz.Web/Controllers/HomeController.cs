using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quiz.Services;
using Quiz.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuizService quizService;

        public HomeController(IQuizService quizService)
        {
            this.quizService = quizService;
        }

        public IActionResult Index()
        {
            var username = this.User?.Identity?.Name;
            var userQuizes = this.quizService.GetQuizesByUsername(username);
            return View(userQuizes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
