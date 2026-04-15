using DB_First_EF_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace DB_First_EF_Demo.Controllers
{
    public class NorthwindController : Controller
    {
        public IActionResult SpainCustomers()
        {
            NorthwindContext context = new NorthwindContext();
            var spaincustomers = context.Customers
            .Where(x => x.Country == "Spain")
            .Select(x => new SpainCustomerViewModel
             { CId=x.CustomerId, 
               CName=x.ContactName,
               ComName=x.CompanyName}).ToList();

            return View(spaincustomers);
        }
        public IActionResult searchCustomer(string contactname)
        {
            NorthwindContext cnt=new NorthwindContext();
            var searchcustomers = cnt.Customers
                .Where(x => x.ContactName == contactname)
                .Select(x => new Customer
                {
                 ContactName=x.ContactName,
                 ContactTitle=x.ContactTitle,
                 CompanyName= x.CompanyName,
                });
            var query1 = searchcustomers.Single();
            return View(query1);
        }
        public ActionResult ProductsInCategory(string categoryname)
        {
            NorthwindContext cnt= new NorthwindContext();
            var productsinCategory = cnt.Products.Where(x => x.Category.CategoryName == categoryname).
                Select(x => new ProdCat
                {
                    prodname = x.ProductName,
                    catname = x.Category.CategoryName
                }).ToList();
            return View(productsinCategory);
        }
        public ActionResult OrderRange(string range)
        {
            NorthwindContext cnt = new NorthwindContext();
            var range1 = Convert.ToInt16(range);
            var custOrderCount = cnt.Customers.Where(x=>x.Orders.Count>range1).Select(x=>new Customer
            {
             CustomerId=x.CustomerId,
             ContactName=x.ContactName,
             
            });
            return View(custOrderCount);
        }
        public ActionResult CustomerOrderDetails(string id)
        {
            NorthwindContext cnt = new NorthwindContext();
            var orders = cnt.Orders.Where(o => o.CustomerId == id)
                        .Select(o => new Order{
                               OrderId= o.OrderId,
                               OrderDate= o.OrderDate,
                              }).ToList();

            return View(orders);
        }

    }
}
