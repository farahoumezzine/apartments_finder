using Microsoft.AspNetCore.Mvc;
using miniprojet.Models;
using miniprojet.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using miniprojet.ViewModels;

namespace miniprojet.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Login
        public IActionResult Login()
        {
            if (TempData["SuccessMessage"] != null)
            {
                _logger.LogInformation("Success message set: {Message}", TempData["SuccessMessage"]);
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            _logger.LogInformation("Login attempt for user: {Login}", login);
            var compte = _context.Comptes
                .FirstOrDefault(c => (c.Username == login || c.Email == login) && c.Password == password);

            if (compte == null)
            {
                _logger.LogWarning("Invalid login attempt for user: {Login}", login);
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            HttpContext.Session.SetString("UserId", compte.Id.ToString());
            HttpContext.Session.SetString("Role", compte.Role);
            HttpContext.Session.SetString("Login", compte.Username);
            _logger.LogInformation("Session set - UserId: {UserId}, Role: {Role}, Login: {Login}",
                compte.Id, compte.Role, compte.Username);

            return compte.Role == "Admin" ? RedirectToAction("Index", "Admin") : RedirectToAction("Index", "Appartement");
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.LogInformation("Register attempt for user: {Username}", model.Username);
            if (ModelState.IsValid)
            {
                // Check if username or email already exists
                if (_context.Comptes.Any(c => c.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(model);
                }

                if (_context.Comptes.Any(c => c.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }

                try
                {
                    var compte = new Compte
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Password = model.Password,
                        Role = "User"
                    };

                    _context.Add(compte);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User {Username} registered successfully", compte.Username);
                    TempData["SuccessMessage"] = "Registration successful! Please log in.";
                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 5)
                {
                    _logger.LogError(ex, "Database is locked during registration for user: {Username}", model.Username);
                    ModelState.AddModelError("", "Database is locked. Please try again later.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during registration for user: {Username}", model.Username);
                    ModelState.AddModelError("", $"An error occurred while registering: {ex.Message}");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Validation failed for registration: {Errors}", string.Join(", ", errors));
            }
            _logger.LogWarning("Registration failed for user: {Username}, returning to view", model.Username);
            return View(model);
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _logger.LogInformation("User logged out, session cleared.");
            return RedirectToAction("Login");
        }
    }
}