using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionKids.Models;

namespace ActionKids.Controllers
{
    public class ServiceController : Controller
    {
        private readonly Context _context;

        public ServiceController(Context context)
        {
            _context = context;
        }

        // GET: Service
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                        .Include(s => s.ServiceRecords)
                    	.OrderByDescending(m => m.ServiceStart)
                    	.ToListAsync();
            return View(services);
        }

        // GET: Service/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.ServiceRecords)
                    .ThenInclude(sr => sr.Kid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeStar(int kidId, int serviceId)
        {
            Console.WriteLine("TAKING STAR");
            var ksr = await _context.KidServiceRecords
                .FirstOrDefaultAsync(m => m.KidId == kidId && m.ServiceId == serviceId);

            if (ksr is null) return NotFound();

            if (ksr.Stars > 0)
            {
                ksr.Stars--;
            }
            else // what do do for a kid with no stars left
            {
                ksr.Stars = 3;
            }
            _context.Update(ksr);
            await _context.SaveChangesAsync();

            return PartialView("_RenderStars", ksr.Stars);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPoints(int kidId, int serviceId, int points)
        {
            var ksr = await _context.KidServiceRecords
                .FirstOrDefaultAsync(m => m.KidId == kidId && m.ServiceId == serviceId);

            if (ksr is null)
            {
                return NotFound();
            }

            ksr.PointsEarned += points;
            _context.Update(ksr);
            await _context.SaveChangesAsync();

            return Content("💰 " + ksr.PointsEarned.ToString(), "text/html");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPoints(int kidId, int serviceId)
        {
            var ksr = await _context.KidServiceRecords
                .FirstOrDefaultAsync(m => m.KidId == kidId && m.ServiceId == serviceId);

            if (ksr is null)
            {
                return NotFound();
            }

            ksr.PointsEarned = 0;
            _context.Update(ksr);
            await _context.SaveChangesAsync();

            return Content("💰 " + ksr.PointsEarned.ToString(), "text/html");
        }

        // POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            Service service = new()
            {
                ServiceStart = DateTime.Now
            };
            _context.Add(service);
            await _context.SaveChangesAsync();

            var kids = await _context.Kids.ToListAsync();
            foreach (var kid in kids)
            {
                KidServiceRecord ksr = new()
                {
                    ServiceId = service.Id,
                    KidId = kid.Id
                };
                _context.Add(ksr);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { Id = service.Id });
        }
        // POST: Service/Stop/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stop(int Id)
        {
            Console.WriteLine("Stopping SERVICE");

            var service = await _context.Services
                .Include(s => s.ServiceRecords)
                    .ThenInclude(s => s.Kid)
                .FirstOrDefaultAsync(s => s.Id == Id);
            if (service is null)
            {
                return NotFound();
            }
            service.ServiceStop = DateTime.Now;
            _context.Update(service);
            foreach (var sr in service.ServiceRecords)
            {
                var kid = sr.Kid;
                kid.Points += sr.PointsEarned;
                if (sr.Stars < 3)
                {
                    kid.TotalLostStars += (3 - sr.Stars);
                }
                _context.Update(kid);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { Id = service.Id });
        }

        // POST: Service/Edit/5
        // T
        // o protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceStart")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
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
            return View(service);
        }

        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddKidToService(int kidId, int ServiceId)
        {
            var record = new KidServiceRecord
            {
                KidId = kidId,
                ServiceId = ServiceId
            };


            _context.KidServiceRecords.Add(record);

            await _context.SaveChangesAsync();

            return RedirectToAction(actionName: nameof(Details), routeValues: new { id = ServiceId });

        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
