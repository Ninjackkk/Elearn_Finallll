using System.ComponentModel.DataAnnotations;

namespace onl.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Thumbnail { get; set; }
        public double Price { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
