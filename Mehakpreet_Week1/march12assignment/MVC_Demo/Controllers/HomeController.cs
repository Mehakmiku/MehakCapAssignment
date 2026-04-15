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
            new Employee{ EmployeeID=101, EmpName="Heera", salary=34000, ImageUrl="/Images/Screenshot (29).png",DeptID=30},
            new Employee { EmployeeID=102, EmpName="Meera", salary=94000, ImageUrl="/Images/Screenshot 2026-01-07 094846.png",DeptID=20}
        };
        
        public IActionResult collectionofdepts()
        {
            return View(deptlist);
        }
        public IActionResult EmpsInDept(int deptid)
        {
            var deptemp =emplist.Where(x=>x.DeptID==deptid).ToList();
            return View(deptemp);
        }

        List<Department> deptlist = new List<Department>()
        {
         new Department{DeptID=10,DeptName="Sales"},
         new Department{DeptID=20,DeptName="HR"},
         new Department{DeptID=30,DeptName="Software"}
        };
        public IActionResult mixedobjectpassing(int empid)
        {
            var query1=deptlist.ToList();
            Employee emp = emplist.Where(x => x.EmployeeID==empid).FirstOrDefault();
            var query2 = emp;
            EmpdeptViewModel obj = new EmpdeptViewModel()
            {
                deptlist = query1,
                emp = query2,
                date = DateTime.Now,
            };
            return View(obj);
        }
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
        public IActionResult Details(int id)
        {
            var employee = emplist.FirstOrDefault(e => e.EmployeeID == id);
            if (employee == null) return NotFound();
            return View(employee);
        }
        public IActionResult Searchemp(int empid)
        {
            Employee emp=emplist.Where(e1=>e1.EmployeeID== empid).FirstOrDefault();
            return View(emp);
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
