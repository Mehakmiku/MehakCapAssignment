namespace UniversityStudentCourse_List.Models
{
    public class StudentDetailVM
    {
        public string Name { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;

        public List<CourseDetailVM> Courses { get; set; } = new();
    }
}
