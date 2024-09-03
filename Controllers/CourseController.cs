using Microsoft.AspNetCore.Mvc;
using onl.Data;
using onl.Models;

namespace onl.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        public CourseController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public IActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCourse(Coursethumb courseThumb)
        {
            if (ModelState.IsValid)
            {
                var path = env.WebRootPath;
                var filePath = "Content/Images/" + courseThumb.Thumbnail.FileName;
                var fullPath = Path.Combine(path, filePath);
                UploadFile(courseThumb.Thumbnail, fullPath);

                var course = new Course
                {
                    Name = courseThumb.Name,
                    Thumbnail = filePath,
                    Price = courseThumb.Price,
                    Description = courseThumb.Description
                };

                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("AddCourse");
            }
            return View(courseThumb);
        }

        public void UploadFile(IFormFile file, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
        [HttpGet]
        public IActionResult GetCourses(int? id)
        {
            if (id.HasValue)
            {

                var course = db.Courses.FirstOrDefault(c => c.Id == id.Value);

                return View("CourseDetails", course);
            }
            else
            {

                var courses = db.Courses.ToList();
                return View("CourseList", courses);
            }
        }

    }



}
