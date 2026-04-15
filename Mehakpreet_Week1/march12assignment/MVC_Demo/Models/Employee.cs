namespace MVC_Demo.Models
{
    public class Employee
    {
        public int EmployeeID { set; get; }
        public string? EmpName { set; get; }
        public int salary { set;  get; }
        public string? ImageUrl { set; get; }

        //FK+reference
        public int DeptID { set; get; }
        public Department? Dept { set; get; }
    }
}
