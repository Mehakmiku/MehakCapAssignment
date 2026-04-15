using Microsoft.AspNetCore.Mvc;
using UniversityStudentCourse_List.Models;
using System.Linq;

namespace UniversityStudentCourse_List.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly List<Student> _students;
        private readonly List<Course> _courses;


        public EnrollmentController()
        {
            // Simplified sample data (10+ enrollments)
            _courses = new()
            {
                new() { CourseId = 1, Title = "Data Structures", Credits = 4, Department = "CSE" },
                new() { CourseId = 2, Title = "Algorithms", Credits = 4, Department = "CSE" },
                new() { CourseId = 3, Title = "Databases", Credits = 3, Department = "CSE" },
                new() { CourseId = 4, Title = "Web Dev", Credits = 3, Department = "IT" },
                new() { CourseId = 5, Title = "OS", Credits = 4, Department = "CSE" }
            };

            _students = new()
            {
                new() { StudentId = 1, Name = "Alice", Branch = "CSE", Enrollments = new()
                {
                    new() { CourseId = 1, Grade = "A", AttemptNumber = 1 },
                    new() { CourseId = 2, Grade = "A-", AttemptNumber = 1 },
                    new() { CourseId = 3, Grade = "B+", AttemptNumber = 1 }
                }},
                new() { StudentId = 2, Name = "Bob", Branch = "CSE", Enrollments = new()
                {
                    new() { CourseId = 1, Grade = "B", AttemptNumber = 1 },
                    new() { CourseId = 4, Grade = "A", AttemptNumber = 1 },
                    new() { CourseId = 5, Grade = "B+", AttemptNumber = 1 }
                }},
                new() { StudentId = 3, Name = "Charlie", Branch = "IT", Enrollments = new()
                {
                    new() { CourseId = 4, Grade = "C", AttemptNumber = 1 },
                    new () { CourseId = 1, Grade = "B-", AttemptNumber = 1 }
                }},
                new() { StudentId = 4,Name = "Diana", Branch = "CSE", Enrollments = new()
                {
                    new() { CourseId = 2, Grade = "A", AttemptNumber = 1 },
                    new() { CourseId = 5, Grade = "F", AttemptNumber = 1 },
                    new() { CourseId = 5, Grade = "B", AttemptNumber = 2 }  // Retry
                }},
                new() { StudentId = 5, Name = "Eve", Branch = "IT", Enrollments = new() }  // No courses
            };
        }

        public IActionResult Index()
        {

            var studentCourses = _students.Select(x => new StudentCoursesVM
            {
                Name = x.Name,
                Branch = x.Branch,
                CourseTitles = x.Enrollments.Select(y => _courses.FirstOrDefault(z => z.CourseId == y.CourseId).Title).ToList(),

            }).ToList();
            return View(studentCourses);
        }
        public IActionResult Details(int studentId)
        {
            var studentinfo = _students.FirstOrDefault(x => x.StudentId == studentId);
            if (studentinfo == null) return NotFound();

            var info = new StudentDetailVM
            {
                Name = studentinfo.Name,
                Branch = studentinfo.Branch,
                Courses = studentinfo.Enrollments.Select(e =>
                {
                    var course = _courses.FirstOrDefault(c => c.CourseId == e.CourseId);
                    return new CourseDetailVM
                    {
                        Title = course.Title,
                        Credits = course.Credits,
                        Department = course.Department,
                        LatestGrade = e.Grade
                    };
                }).ToList()

            };
            return View(info);
        }
    }
}
