namespace onl.Models
{
    public class WatchCourse
    {
        public Course Course { get; set; }
        public List<Upload> Videos { get; set; }

        public List<MCQ> MCQs { get; set; }  // New property for MCQs
        public Assignment Assignment { get; set; }  // New property for Assignment
    }
}
