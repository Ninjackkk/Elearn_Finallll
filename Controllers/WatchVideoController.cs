using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using onl.Data;
using onl.Models;

namespace onl.Controllers
{
    public class WatchVideoController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly IConfiguration _configuration;

        public WatchVideoController(ApplicationDbContext db, IConfiguration configuration)
        {
            this.db = db;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }



        //public IActionResult PlayVideo(string Courses)
        //{
        //    //var video = db.Uploads
        //    //    .Where(v => v.Courses == Courses)
        //    //    .Select(v => new Upload
        //    //    {
        //    //        Id = v.Id,
        //    //        TopicName = v.TopicName,
        //    //        YouTubeLink = v.YouTubeLink,
        //    //        VideoFile = v.VideoFile
        //    //    })
        //    //    .FirstOrDefault();



        //        var video = db.Uploads
        //            .AsEnumerable()  // Switch to client-side evaluation
        //            .Where(v => string.Equals(v.Courses, Courses, StringComparison.OrdinalIgnoreCase))
        //            .Select(v => new Upload
        //            {
        //                //Id = v.Id,
        //                TopicName = v.TopicName,
        //                YouTubeLink = v.YouTubeLink,
        //                VideoFile = v.VideoFile
        //            })
        //            .ToList();



        //        if (video == null)
        //    {
        //        return NotFound();
        //    }




        //    return View(video);
        //}



        public IActionResult PlayVideo(string Courses)
        {
            var normalizedCourse = Courses.Trim().ToLower();

            var video = db.Uploads
                .AsEnumerable()  // Switch to client-side evaluation
                .Select(v => new
                {
                    v.Id,
                    v.TopicName,
                    v.YouTubeLink,
                    v.VideoFile,
                    NormalizedCourses = v.Courses.Trim().ToLower()  // Normalize case and trim spaces
                })
                .SingleOrDefault(v => string.Equals(v.NormalizedCourses, normalizedCourse, StringComparison.OrdinalIgnoreCase));

            if (video == null)
            {
                return NotFound();
            }

            // Create a single Upload object to pass to the view
            var upload = new Upload
            {
                Id = video.Id,
                TopicName = video.TopicName,
                YouTubeLink = video.YouTubeLink,
                VideoFile = video.VideoFile
            };

            return View(upload);
        }

    }
}
