using Microsoft.AspNetCore.Mvc;
using onl.Data;
using onl.Models;


public class CartController : Controller
{
    private readonly ApplicationDbContext db;
    private readonly IConfiguration _configuration;

    public CartController(ApplicationDbContext db, IConfiguration configuration)
    {
        this.db = db;
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult AddToCart(int courseId)
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var course = db.Courses.FirstOrDefault(c => c.Id == courseId);
        if (course != null)
        {
            var cartItem = new Cart
            {
                Name = course.Name,
                Thumbnail = course.Thumbnail,
                Price = course.Price,
                Description = course.Description,
                suser = username,
                IsPurchased = null
            };

            db.Carts.Add(cartItem);
            db.SaveChanges();
        }

        return RedirectToAction("ViewCart");
    }

    public IActionResult ViewCart()
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null)
        {
            return RedirectToAction("SignIn", "Auth");
        }
        var cartItems = db.Carts.Where(c => c.suser == username && c.IsPurchased == null).ToList();
        var totalAmount = cartItems.Sum(c => c.Price);
        HttpContext.Session.SetString("total",totalAmount.ToString());

        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> RazorpayCheckout()
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var cartItems = db.Carts.Where(c => c.suser == username && c.IsPurchased == null).ToList();
        if (cartItems.Any())
        {
            // Generate a random order ID
            var randomOrderId = Guid.NewGuid().ToString();

            foreach (var item in cartItems)
            {
                item.IsPurchased = "True"; // Mark as purchased
                item.OrderId = randomOrderId;
            }

            db.Carts.UpdateRange(cartItems);
            db.SaveChanges();

            // Pass the order ID and amount to the view
            ViewBag.OrderId = randomOrderId;
            ViewBag.Amount = cartItems.Sum(c => c.Price) * 100; // Total amount in paise
            return View("RazorpayCheckout");
        }

        return RedirectToAction("ViewCart");
    }

    public IActionResult MyCourses()
    {
        var username = HttpContext.Session.GetString("username");
        if (username == null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var purchasedCourses = db.Carts.Where(c => c.suser == username && c.IsPurchased == "True").ToList();
        return View(purchasedCourses);
    }
}
