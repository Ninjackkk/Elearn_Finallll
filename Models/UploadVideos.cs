//using System.ComponentModel.DataAnnotations;
//using System.Diagnostics.CodeAnalysis;

//namespace onl.Models
//{
//    public class UploadVideos
//    {
//        [Key]
//        public int Id { get; set; }
//        [Required]
//        public string Courses { get; set; }

//        [Required]
//        public string TopicName { get; set; }
//        [AllowNull]
//        public IFormFile VideoFile { get; set; }

//        public string YouTubeLink { get; set; }
//    }
//}


// Previous code without mcqs and assignment (changes made in uploadvideos controller , upload and uploadvideo model
// uploadvideo.cshtml and applicationdbcontext


using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace onl.Models
{
    public class UploadVideos
    {
        [Required]
        public string? Courses { get; set; }

        [Required]
        public string? TopicName { get; set; }

        [Required]
        public IFormFile VideoFile { get; set; }

        [Required]
        public string? YouTubeLink { get; set; }

        // New properties for MCQs and Assignment
        public List<MCQ> MCQs { get; set; } = new List<MCQ>();
        public Assignment? Assignment { get; set; }
    }
}
