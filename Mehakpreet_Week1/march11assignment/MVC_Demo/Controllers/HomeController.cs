using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_Demo.Models;

namespace MVC_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public string sampledemo1()
        {
            return "Mehak";
        }

        public string sampledemo2(int age,string name)
        {
            return "The name " + name + " having age" + age;
        }

        public IActionResult sampledemo3()
        {
            int age = 34;
            string name = "Mikku";
            ViewBag.Name = name;
            ViewBag.Age = age;
            ViewData["Message"] = "Welcome to ASP.net core learning";
            ViewData["year"] = DateTime.Now.Year;
            return View();
        }
        Employee obj = new Employee()
        {
            EmployeeID=101,
            EmpName="Mehar",
            salary=70000
        };

        List<Employee> emplist = new List<Employee>()
        {
            new Employee{ EmployeeID=101, EmpName="Heera", salary=34000, ImageUrl="/Images/Screenshot (29).png"},
            new Employee { EmployeeID=102, EmpName="Meera", salary=94000, ImageUrl="/Images/Screenshot 2026-01-07 094846.png"}
        };
        public IActionResult collectionofobjectpassing()
        {
            return View(emplist);
        }
        public IActionResult singleobjectpassing()
        {
            return View(obj);
        }
        public IActionResult Display()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
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
