using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using onl.Data;
using onl.Models;
//ehehe
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


        //     public IActionResult PlayVideo(string Courses)
        //{
        //    if (string.IsNullOrWhiteSpace(Courses))
        //    {
        //        return BadRequest("Course name cannot be empty.");
        //    }

        //    var normalizedCourse = Courses.Trim().ToLower();

        //    // Fetch the course and its videos
        //    var course = db.Courses
        //        .AsEnumerable()  // Assuming `Courses` is a DbSet in your ApplicationDbContext
        //        .SingleOrDefault(c => string.Equals(c.Name.Trim().ToLower(), normalizedCourse, StringComparison.OrdinalIgnoreCase));

        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    var videos = db.Uploads
        //        .AsEnumerable()  // Fetch the associated videos
        //        .Where(v => string.Equals(v.Courses.Trim().ToLower(), normalizedCourse, StringComparison.OrdinalIgnoreCase))
        //        .Select(v => new Upload
        //        {
        //            Id = v.Id,
        //            TopicName = v.TopicName,
        //            YouTubeLink = v.YouTubeLink,
        //            VideoFile = v.VideoFile
        //        })
        //        .ToList();

        //    if (!videos.Any())
        //    {
        //        return NotFound();
        //    }

        //    // Create the view model
        //    var viewModel = new WatchCourse
        //    {
        //        Course = course,
        //        Videos = videos
        //    };

        //    return View(viewModel);
        //}

        public IActionResult PlayVideo(string Courses)
        {
            if (string.IsNullOrWhiteSpace(Courses))
            {
                return BadRequest("Course name cannot be empty.");
            }

            var normalizedCourse = Courses.Trim().ToLower();

            // Fetch the course and its videos
            var course = db.Courses
                .AsEnumerable()  // Fetch all courses and then filter
                .SingleOrDefault(c => string.Equals(c.Name.Trim().ToLower(), normalizedCourse, StringComparison.OrdinalIgnoreCase));

            if (course == null)
            {
                return NotFound();
            }

            var videos = db.Uploads
                .AsEnumerable()  // Fetch all uploads and then filter
                .Where(v => string.Equals(v.Courses.Trim().ToLower(), normalizedCourse, StringComparison.OrdinalIgnoreCase))
                .Select(v => new Upload
                {
                    Id = v.Id,
                    TopicName = v.TopicName,
                    YouTubeLink = v.YouTubeLink,
                    VideoFile = v.VideoFile
                })
                .ToList();

            if (!videos.Any())
            {
                return NotFound();
            }

            // Fetch MCQs and Assignment for the first video
            var mainVideo = videos.First();
            var mcqs = db.MCQs
                .Where(mcq => mcq.UploadId == mainVideo.Id)
                .ToList();

            var assignment = db.Assignments
                .SingleOrDefault(a => a.UploadId == mainVideo.Id);

            // Create the view model
            var viewModel = new WatchCourse
            {
                Course = course,
                Videos = videos,
                MCQs = mcqs,
                Assignment = assignment
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GetVideoDetails(int videoId)
        {
            var video = db.Uploads
                .Include(v => v.MCQs)
                .Include(v => v.Assignment)
                .SingleOrDefault(v => v.Id == videoId);

            if (video == null)
            {
                return NotFound();
            }

            var mcqs = video.MCQs.ToList();
            var assignment = video.Assignment;

            var result = new
            {
                Video = new
                {
                    video.Id,
                    video.YouTubeLink
                },
                MCQs = mcqs,
                Assignment = assignment
            };

            return Json(result);
        }


    }
}
