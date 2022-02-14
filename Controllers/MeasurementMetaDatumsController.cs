using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_capture.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Data_capture.Controllers
{
    public class MeasurementMetaDatumsController : Controller
    {
        DateTime localDate = DateTime.Now;

        private readonly DataCaptureContext _context;

        public MeasurementMetaDatumsController(DataCaptureContext context)
        {
            _context = context;
        }

        // GET: MeasurementMetaDatums
        public async Task<IActionResult> Index()
        {
            var dataCaptureContext = _context.MeasurementMetaData.Include(m => m.MIdNavigation);
            return View(await dataCaptureContext.ToListAsync());
        }

        // GET: MeasurementMetaDatums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurementMetaDatum = await _context.MeasurementMetaData
                .Include(m => m.MIdNavigation)
                .FirstOrDefaultAsync(m => m.MmdId == id);
            if (measurementMetaDatum == null)
            {
                return NotFound();
            }

            return View(measurementMetaDatum);
        }

        // GET: MeasurementMetaDatums/Create
        public IActionResult Create()
        {
            ViewData["MId"] = new SelectList(_context.Measurements, "MId", "UserId");
            return View();
        }

        // POST: MeasurementMetaDatums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MmdId,MId,MmdTimeStamp,MmdStatus")] MeasurementMetaDatum measurementMetaDatum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(measurementMetaDatum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MId"] = new SelectList(_context.Measurements, "MId", "UserId", measurementMetaDatum.MId);
            return View(measurementMetaDatum);
        }

        // GET: MeasurementMetaDatums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurementMetaDatum = await _context.MeasurementMetaData.FindAsync(id);
            if (measurementMetaDatum == null)
            {
                return NotFound();
            }
            //ViewData["MId"] = new SelectList(_context.Measurements, "MId", "UserId", measurementMetaDatum.MId);
            return View(measurementMetaDatum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MmdId,MId,MmdTimeStamp,MmdStatus")] MeasurementMetaDatum measurementMetaDatum)
        {
            if (id != measurementMetaDatum.MmdId)
            {
                return NotFound();
            }

            //Is user signed in?
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("currentClientId")))
            {
                return RedirectToAction("login", "AspNetUsers");
            }

            try
            {
#warning Failed to convert localDate to MmdTimeStamp
                measurementMetaDatum.MmdTimeStamp = Encoding.ASCII.GetBytes(localDate.ToString());

                _context.Update(measurementMetaDatum);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasurementMetaDatumExists(measurementMetaDatum.MmdId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            return View(measurementMetaDatum);
        }

        // GET: MeasurementMetaDatums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurementMetaDatum = await _context.MeasurementMetaData
                .Include(m => m.MIdNavigation)
                .FirstOrDefaultAsync(m => m.MmdId == id);
            if (measurementMetaDatum == null)
            {
                return NotFound();
            }

            return View(measurementMetaDatum);
        }

        // POST: MeasurementMetaDatums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var measurementMetaDatum = await _context.MeasurementMetaData.FindAsync(id);
            _context.MeasurementMetaData.Remove(measurementMetaDatum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeasurementMetaDatumExists(int id)
        {
            return _context.MeasurementMetaData.Any(e => e.MmdId == id);
        }
    }
}
