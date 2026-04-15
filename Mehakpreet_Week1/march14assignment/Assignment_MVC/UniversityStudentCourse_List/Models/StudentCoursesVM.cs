namespace UniversityStudentCourse_List.Models
{
    public class StudentCoursesVM
    {
        public string Name { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public List<string> CourseTitles { get; set; } = new();
        public int CourseCount => CourseTitles.Count;
    }
}
