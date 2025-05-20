using Microsoft.AspNetCore.Mvc;
using miniprojet.Data;
using miniprojet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace miniprojet.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin Dashboard
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // GET: List all apartments
        public async Task<IActionResult> ManageApartments()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var apartments = await _context.Appartements
                .Include(a => a.Propriétaire)
                .ToListAsync();
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            return View(apartments);
        }

        // GET: Create new apartment
        public IActionResult CreateApartment()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var proprietors = _context.Propriétaires.ToList();
            if (proprietors == null || !proprietors.Any())
            {
                ModelState.AddModelError("", "No proprietors available. Please add a proprietor first.");
                return View();
            }
            ViewBag.Proprietaires = proprietors.Select(p => new { p.IdProp, FullName = $"{p.Nom} {p.Prénom} (ID: {p.IdProp})" });
            return View();
        }

        // POST: Create new apartment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApartment(Appartement appartement)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation("POST CreateApartment called with: {@Appartement}", appartement);

            if (ModelState.IsValid || (appartement.IdProp > 0))
            {
                try
                {
                    _logger.LogInformation("IdProp value before save: {IdProp}", appartement.IdProp);
                    _context.Add(appartement);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Apartment created successfully with NumApp: {NumApp}", appartement.NumApp);
                    return RedirectToAction(nameof(ManageApartments));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving apartment: {@Appartement}. Inner Exception: {InnerException}", appartement, ex.InnerException?.Message ?? "No inner exception");
                    ModelState.AddModelError("", "An error occurred while saving the apartment: " + (ex.InnerException?.Message ?? ex.Message));
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct();
                _logger.LogWarning("ModelState invalid for apartment creation. Errors: {@Errors}", errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            ViewBag.Proprietaires = _context.Propriétaires.ToList().Select(p => new { p.IdProp, FullName = $"{p.Nom} {p.Prénom} (ID: {p.IdProp})" });
            return View(appartement);
        }

        // GET: Edit apartment
        public async Task<IActionResult> EditApartment(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                _logger.LogWarning("EditApartment GET called with null id");
                return NotFound();
            }

            var appartement = await _context.Appartements.FindAsync(id);
            if (appartement == null)
            {
                _logger.LogWarning("Apartment not found for id: {Id}", id);
                return NotFound();
            }

            ViewBag.Proprietaires = _context.Propriétaires.ToList().Select(p => new { p.IdProp, FullName = $"{p.Nom} {p.Prénom} (ID: {p.IdProp})" });
            return View(appartement);
        }

        // POST: Edit apartment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApartment(int id, Appartement appartement)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation("POST EditApartment called with id: {Id}, Appartement: {@Appartement}", id, appartement);

            // Log raw form data for debugging
            var formData = HttpContext.Request.Form;
            _logger.LogInformation("Form data received: {@FormData}", formData.ToDictionary(fd => fd.Key, fd => fd.Value.ToString()));

            if (id != appartement.NumApp)
            {
                _logger.LogWarning("Id mismatch in EditApartment. URL id: {Id}, Appartement.NumApp: {NumApp}", id, appartement.NumApp);
                return NotFound();
            }

            // Temporary diagnostic bypass
            if (appartement.IdProp > 0)
            {
                try
                {
                    _logger.LogInformation("Updating apartment with NumApp: {NumApp}, IdProp: {IdProp}", appartement.NumApp, appartement.IdProp);
                    _context.Update(appartement);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Apartment updated successfully with NumApp: {NumApp}", appartement.NumApp);
                    TempData["SuccessMessage"] = "Apartment updated successfully!";
                    return RedirectToAction(nameof(ManageApartments));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error updating apartment: {@Appartement}", appartement);
                    if (!_context.Appartements.Any(e => e.NumApp == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating apartment: {@Appartement}. Inner Exception: {InnerException}", appartement, ex.InnerException?.Message ?? "No inner exception");
                    ModelState.AddModelError("", "An error occurred while updating the apartment: " + (ex.InnerException?.Message ?? ex.Message));
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Updating apartment with NumApp: {NumApp}, IdProp: {IdProp}", appartement.NumApp, appartement.IdProp);
                    _context.Update(appartement);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Apartment updated successfully with NumApp: {NumApp}", appartement.NumApp);
                    TempData["SuccessMessage"] = "Apartment updated successfully!";
                    return RedirectToAction(nameof(ManageApartments));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error updating apartment: {@Appartement}", appartement);
                    if (!_context.Appartements.Any(e => e.NumApp == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating apartment: {@Appartement}. Inner Exception: {InnerException}", appartement, ex.InnerException?.Message ?? "No inner exception");
                    ModelState.AddModelError("", "An error occurred while updating the apartment: " + (ex.InnerException?.Message ?? ex.Message));
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct();
                _logger.LogWarning("ModelState invalid for apartment update. Errors: {@Errors}", errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            ViewBag.Proprietaires = _context.Propriétaires.ToList().Select(p => new { p.IdProp, FullName = $"{p.Nom} {p.Prénom} (ID: {p.IdProp})" });
            return View(appartement);
        }

        // GET: Delete apartment
        public async Task<IActionResult> DeleteApartment(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var appartement = await _context.Appartements
                .Include(a => a.Propriétaire)
                .FirstOrDefaultAsync(m => m.NumApp == id);
            if (appartement == null)
            {
                return NotFound();
            }

            return View(appartement);
        }

        // POST: Delete apartment
        [HttpPost, ActionName("DeleteApartment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteApartmentConfirmed(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role")) || HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var appartement = await _context.Appartements.FindAsync(id);
            if (appartement != null)
            {
                _context.Appartements.Remove(appartement);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageApartments));
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            return RedirectToAction("Login", "Account");
        }
    }
}