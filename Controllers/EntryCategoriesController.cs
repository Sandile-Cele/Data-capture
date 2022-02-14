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
    public class EntryCategoriesController : Controller
    {
        private readonly DataCaptureContext _context;

        public EntryCategoriesController(DataCaptureContext context)
        {
            _context = context;
        }

        // GET: EntryCategories
        public async Task<IActionResult> Index()
        {
            var dataCaptureContext = _context.EntryCategories.Include(e => e.User);
            return View(await dataCaptureContext.ToListAsync());
        }

        // GET: EntryCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryCategory = await _context.EntryCategories
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EcId == id);
            if (entryCategory == null)
            {
                return NotFound();
            }

            return View(entryCategory);
        }

        // GET: EntryCategories/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: EntryCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EcId,UserId,EcName")] EntryCategory entryCategory)
        {
            if (ModelState.IsValid)
            {
                //If user is not logged in, send to login page
                if(HttpContext.Session.GetString("currentClientId") == null)
                {
                    return RedirectToAction("login", "aspnetusers");
                }

                entryCategory.UserId = HttpContext.Session.GetString("currentClientId");

                _context.Add(entryCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", entryCategory.UserId);
            return View(entryCategory);
        }

        // GET: EntryCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryCategory = await _context.EntryCategories.FindAsync(id);
            if (entryCategory == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", entryCategory.UserId);
            return View(entryCategory);
        }

        // POST: EntryCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EcId,UserId,EcName")] EntryCategory entryCategory)
        {
            if (id != entryCategory.EcId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entryCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryCategoryExists(entryCategory.EcId))
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", entryCategory.UserId);
            return View(entryCategory);
        }

        // GET: EntryCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryCategory = await _context.EntryCategories
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EcId == id);
            if (entryCategory == null)
            {
                return NotFound();
            }

            return View(entryCategory);
        }

        // POST: EntryCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entryCategory = await _context.EntryCategories.FindAsync(id);
            _context.EntryCategories.Remove(entryCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntryCategoryExists(int id)
        {
            return _context.EntryCategories.Any(e => e.EcId == id);
        }
    }
}
