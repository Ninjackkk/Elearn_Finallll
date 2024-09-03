//using System.ComponentModel.DataAnnotations;

//namespace onl.Models
//{
//    public class Upload
//    {
//        [Key]
//        public int Id { get; set; }
//        [Required]
//        public string? Courses { get; set; }

//        [Required]
//        public string? TopicName { get; set; }

//        public string? VideoFile { get; set; }

//        [Required]
//        public string? YouTubeLink { get; set; }

//    }
//}

// Previous code without mcqs and assignment (changes made in uploadvideos controller , upload and uploadvideo model
// uploadvideo.cshtml and applicationdbcontext


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace onl.Models
{
    public class Upload
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Courses { get; set; }

        [Required]
        public string? TopicName { get; set; }

        public string? VideoFile { get; set; }

        [Required]
        public string? YouTubeLink { get; set; }

        // New properties for MCQs and Assignment
        public virtual ICollection<MCQ>? MCQs { get; set; }
        public Assignment? Assignment { get; set; }
    }

    public class MCQ
    {
        [Key]
        public int Id { get; set; }
        public int UploadId { get; set; }
        [Required]
        public string Question { get; set; } = string.Empty;
        [Required]
        public string OptionA { get; set; } = string.Empty;
        [Required]
        public string OptionB { get; set; } = string.Empty;
        [Required]
        public string OptionC { get; set; } = string.Empty;
        [Required]
        public string OptionD { get; set; } = string.Empty;
        [Required]
        public string Answer { get; set; } = string.Empty;

        public virtual Upload? Upload { get; set; }
    }

    public class Assignment
    {
        [Key]
        public int Id { get; set; }
        public int UploadId { get; set; }
        [Required]
        public string Question { get; set; } = string.Empty;

        public virtual Upload? Upload { get; set; }
    }
}

