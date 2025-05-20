using Microsoft.AspNetCore.Mvc;
using miniprojet.Data;
using miniprojet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace miniprojet.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");

            var stats = new
            {
                TotalLocations = _context.Locations.Count(),
                TotalMontant = _context.Locations.Sum(l => l.Montant)
            };

            return View(stats);
        }

        // CRUD for Appartement
        public async Task<IActionResult> ManageAppartements()
        {
            return View(await _context.Appartements.Include(a => a.Propriétaire).ToListAsync());
        }

        public IActionResult CreateAppartement()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppartement(Appartement appartement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appartement);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageAppartements");
            }
            return View(appartement);
        }

        // GET: Edit Appartement
        public async Task<IActionResult> EditAppartement(int? id)
        {
            if (id == null)
                return NotFound();

            var appartement = await _context.Appartements.FindAsync(id);
            if (appartement == null)
                return NotFound();

            return View(appartement);
        }

        // POST: Edit Appartement
        [HttpPost]
        public async Task<IActionResult> EditAppartement(int id, Appartement appartement)
        {
            if (id != appartement.NumApp)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appartement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Appartements.Any(e => e.NumApp == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("ManageAppartements");
            }
            return View(appartement);
        }

        // GET: Delete Appartement
        public async Task<IActionResult> DeleteAppartement(int? id)
        {
            if (id == null)
                return NotFound();

            var appartement = await _context.Appartements
                .Include(a => a.Propriétaire)
                .FirstOrDefaultAsync(m => m.NumApp == id);
            if (appartement == null)
                return NotFound();

            return View(appartement);
        }

        // POST: Delete Appartement
        [HttpPost, ActionName("DeleteAppartement")]
        public async Task<IActionResult> DeleteAppartementConfirmed(int id)
        {
            var appartement = await _context.Appartements.FindAsync(id);
            if (appartement != null)
            {
                _context.Appartements.Remove(appartement);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageAppartements");
        }
    }
}

