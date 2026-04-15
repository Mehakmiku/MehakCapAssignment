namespace UniversityStudentCourse_List.Models
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string Grade { get; set; } = string.Empty;
        public int AttemptNumber { get; set; }
    }
}
