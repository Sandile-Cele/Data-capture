using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_capture.Models;

namespace Data_capture.Controllers
{
    public class MeasurementsController : Controller
    {
        private readonly DataCaptureContext _context;

        public MeasurementsController(DataCaptureContext context)
        {
            _context = context;
        }

        // GET: Measurements
        public async Task<IActionResult> Index()
        {
            var dataCaptureContext = _context.Measurements.Include(m => m.Ec).Include(m => m.User);
            return View(await dataCaptureContext.ToListAsync());
        }

        // GET: Measurements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurement = await _context.Measurements
                .Include(m => m.Ec)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MId == id);
            if (measurement == null)
            {
                return NotFound();
            }

            return View(measurement);
        }

        // GET: Measurements/Create
        public IActionResult Create()
        {
            ViewData["EcId"] = new SelectList(_context.EntryCategories, "EcId", "EcName");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Measurements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MId,UserId,EcId,MTemperature,MHumidity,MWeight,MWidth,MLength,MDepth")] Measurement measurement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(measurement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EcId"] = new SelectList(_context.EntryCategories, "EcId", "EcName", measurement.EcId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", measurement.UserId);
            return View(measurement);
        }

        // GET: Measurements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                return NotFound();
            }
            ViewData["EcId"] = new SelectList(_context.EntryCategories, "EcId", "EcName", measurement.EcId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", measurement.UserId);
            return View(measurement);
        }

        // POST: Measurements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MId,UserId,EcId,MTemperature,MHumidity,MWeight,MWidth,MLength,MDepth")] Measurement measurement)
        {
            if (id != measurement.MId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(measurement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasurementExists(measurement.MId))
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
            ViewData["EcId"] = new SelectList(_context.EntryCategories, "EcId", "EcName", measurement.EcId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", measurement.UserId);
            return View(measurement);
        }

        // GET: Measurements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurement = await _context.Measurements
                .Include(m => m.Ec)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MId == id);
            if (measurement == null)
            {
                return NotFound();
            }

            return View(measurement);
        }

        // POST: Measurements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeasurementExists(int id)
        {
            return _context.Measurements.Any(e => e.MId == id);
        }
    }
}
