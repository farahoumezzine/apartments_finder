using Microsoft.AspNetCore.Mvc;
using miniprojet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using miniprojet.Models;

namespace miniprojet.Controllers
{
    public class AppartementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppartementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Appartement/Index
        public async Task<IActionResult> Index(string searchLocalite = null, decimal? minValeur = null)
        {
            // Prevent caching
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            // Check if user is authenticated
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                ViewBag.WelcomeMessage = $"Welcome, {HttpContext.Session.GetString("Username")}! You are logged in as {HttpContext.Session.GetString("Role")}";
            }

            var appartements = _context.Appartements
                .Include(a => a.Propriétaire)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchLocalite))
                appartements = appartements.Where(a => a.Localite.Contains(searchLocalite));

            if (minValeur.HasValue)
                appartements = appartements.Where(a => a.Valeur >= minValeur.Value);

            var model = await appartements.ToListAsync();
            return View(model);
        }

        // GET: Rent apartment
        public async Task<IActionResult> Rent(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var appartement = await _context.Appartements.FindAsync(id);
            if (appartement == null)
                return NotFound();

            // Simulate rental by creating a Location record
            var location = new Location
            {
                IdLoc = int.Parse(userId), // Assuming user is Locataire
                NumApp = id,
                DatLoc = DateTime.Now,
                NbrMois = 12,
                Montant = appartement.Valeur
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}