using Microsoft.AspNetCore.Mvc;
using onl.Data;
using onl.Models; 

namespace onl.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext db;

        public TestController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var data = db.Courses.ToList();
            return View(data);
        }

        public async Task<IActionResult> CourseSingle(int id)
        {
            var course = await db.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            else
            {
                return View(course);
            }
            
        }


    }
}
