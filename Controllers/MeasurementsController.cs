using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_capture.Models;
using Microsoft.AspNetCore.Http;

namespace Data_capture.Controllers
{
    public class MeasurementsController : Controller
    {
        private readonly DataCaptureContext _context;

        public MeasurementsController(DataCaptureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Only user who created can edit
            ViewBag.currentUserId = HttpContext.Session.GetString("currentClientId");

            var dataCaptureContext = _context.Measurements.Include(m => m.Ec).Include(m => m.User);
            return View(await dataCaptureContext.ToListAsync());
        }

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

        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("currentClientId")))
            {
                return RedirectToAction("login", "AspNetUsers");
            }

            ViewData["EcId"] = new SelectList(_context.EntryCategories, "EcId", "EcName");
            //ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MId,UserId,EcId,MTemperature,MHumidity,MWeight,MWidth,MLength,MDepth")] Measurement measurement)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("currentClientId")))
            {
                return RedirectToAction("login", "AspNetUsers");
            }

            measurement.UserId = HttpContext.Session.GetString("currentClientId");

            try
            {
                _context.Add(measurement);
                await _context.SaveChangesAsync();

                //User to complete metadata

                return RedirectToAction("create", "MeasurementMetaDatums");

            }
            catch (Exception)
            {
                ViewData["EcId"] = new SelectList(_context.EntryCategories, "EcId", "EcName", measurement.EcId);

                return View(measurement);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("currentClientId")))
            {
                return RedirectToAction("login", "AspNetUsers");
            }

            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                return NotFound();
            }

            //If this is NOT the user who created this entry, then send to login!
            if (!measurement.UserId.Equals(HttpContext.Session.GetString("currentClientId")))
            {
                return RedirectToAction("login", "AspNetUsers");
            }

            ViewData["EcId"] = new SelectList(_context.EntryCategories, "EcId", "EcName", measurement.EcId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", measurement.UserId);
            return View(measurement);
        }

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
