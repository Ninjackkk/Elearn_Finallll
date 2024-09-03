//using Microsoft.AspNetCore.Mvc;
//using onl.Data;
//using onl.Models;

//namespace onl.Controllers
//{
//    public class UploadVideosController : Controller
//    {
//        private readonly ApplicationDbContext db;
//        private readonly IWebHostEnvironment env;

//        public UploadVideosController(ApplicationDbContext db, IWebHostEnvironment env)
//        {
//            this.db = db;
//            this.env = env;
//        }
//        public IActionResult UploadVideo()
//        {
//            var courses = db.Courses.ToList();
//            ViewBag.Courses = courses;
//            return View();
//        }

//        [HttpPost]
//        public IActionResult UploadVideo(UploadVideos v)
//        {
//            var path = env.WebRootPath;
//            var filePath = "Content/Images/" + v.VideoFile.FileName;
//            var fullPath = Path.Combine(path, filePath);
//            UploadFile(v.VideoFile, fullPath);

//            var p = new Upload()
//            {
//                Courses = v.Courses,
//                TopicName = v.TopicName,
//                VideoFile = filePath,
//                YouTubeLink = v.YouTubeLink
//            };
//            db.Add(p);
//            db.SaveChanges();
//            return RedirectToAction("showvideos", "UploadVideos");
//        }

//        public void UploadFile(IFormFile file, string path)
//        {
//            using var stream = new FileStream(path, FileMode.Create);
//            file.CopyTo(stream);
//        }


// Previous code without mcqs and assignment (changes made in uploadvideos controller , upload and uploadvideo model
// uploadvideo.cshtml and applicationdbcontext .



using Microsoft.AspNetCore.Mvc;
using onl.Data;
using onl.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace onl.Controllers
{
    public class UploadVideosController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;

        public UploadVideosController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        public IActionResult UploadVideo()
        {
            var courses = db.Courses.ToList();
            ViewBag.Courses = courses;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadVideo(UploadVideos v)
        {
            if (!ModelState.IsValid)
            {
                return View(v);
            }

            var path = env.WebRootPath;
            var filePath = "Content/Images/" + v.VideoFile.FileName;
            var fullPath = Path.Combine(path, filePath);
            await UploadFile(v.VideoFile, fullPath);

            var upload = new Upload
            {
                Courses = v.Courses,
                TopicName = v.TopicName,
                VideoFile = filePath,
                YouTubeLink = v.YouTubeLink
            };

            // Add the Upload entity to the database
            db.Uploads.Add(upload);
            await db.SaveChangesAsync(); // Save changes to generate the ID

            // Add MCQs
            foreach (var mcq in v.MCQs)
            {
                mcq.UploadId = upload.Id;
                db.MCQs.Add(mcq);
            }

            // Add Assignment
            if (v.Assignment != null)
            {
                v.Assignment.UploadId = upload.Id;
                db.Assignments.Add(v.Assignment);
            }

            await db.SaveChangesAsync(); // Save changes again

            return RedirectToAction("ShowVideos", new { courseId = v.Courses });
        }
        public async Task UploadFile(IFormFile file, string path)
        {
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }


        [Route("courses")]
        public IActionResult ShowCourses()
        {
            var courses = db.Courses.ToList();
            if (courses == null || !courses.Any())
            {
                return View(new List<Course>());
            }
            return View(courses);
        }
        public IActionResult ShowVideos(int? courseId)
        {
            if (courseId == null)
            {
                return RedirectToAction("ShowCourses");
            }

            var selectedCourse = db.Courses.FirstOrDefault(c => c.Id == courseId);
            if (selectedCourse == null)
            {
                return RedirectToAction("ShowCourses");
            }

            var uploads = db.Uploads.Where(u => u.Courses == selectedCourse.Name).ToList();
            ViewBag.Course = selectedCourse;
            return View(uploads);
        }

        public IActionResult EditCourse(int id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public IActionResult EditCourse(Course course, IFormFile newThumbnail)
        {
            if (ModelState.IsValid)
            {
                if (newThumbnail != null)
                {
                    var path = env.WebRootPath;
                    var filePath = "Content/Images/" + newThumbnail.FileName;
                    var fullPath = Path.Combine(path, filePath);


                    if (!string.IsNullOrEmpty(course.Thumbnail))
                    {
                        var oldThumbnailPath = Path.Combine(path, course.Thumbnail);
                        if (System.IO.File.Exists(oldThumbnailPath))
                        {
                            System.IO.File.Delete(oldThumbnailPath);
                        }
                    }

                    UploadFile(newThumbnail, fullPath);
                    course.Thumbnail = filePath;
                }

                else
                {
                    var existing = db.Courses.Where(c => c.Id == course.Id).SingleOrDefault();
                    if (existing != null)
                    {
                        course.Thumbnail = existing.Thumbnail;
                    }
                }

                db.Update(course);
                db.SaveChanges();
                return RedirectToAction("ShowCourses");
            }
            return View(course);
        }

        [HttpPost]
        public IActionResult DeleteCourse(int id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            return RedirectToAction("ShowCourses");


        }
    }
}
