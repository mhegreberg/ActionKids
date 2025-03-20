using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionKids;
using ActionKids.Models;

namespace ActionKids.Controllers
{
    public class KidController : Controller
    {
        private readonly Context _context;

        public KidController(Context context)
        {
            _context = context;
        }

        // GET: Kid
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kids.ToListAsync());
        }

        // GET: Kid/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids
				.Include(k => k.ServiceRecords)
					.ThenInclude(sr => sr.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
            {
                return NotFound();
            }

            return View(kid);
        }

        // GET: Kid/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Birthday,Points,TotalLostStars")] Kid kid)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kid);
        }

        // GET: Kid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids.FindAsync(id);
            if (kid == null)
            {
                return NotFound();
            }
            return View(kid);
        }

        // POST: Kid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Birthday,Points,TotalLostStars")] Kid kid)
        {
            if (id != kid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KidExists(kid.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kid);
        }

        // GET: Kid/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
            {
                return NotFound();
            }

            return View(kid);
        }

        // POST: Kid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kid = await _context.Kids.FindAsync(id);
            if (kid != null)
            {
                _context.Kids.Remove(kid);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KidExists(int id)
        {
            return _context.Kids.Any(e => e.Id == id);
        }
    }
}
