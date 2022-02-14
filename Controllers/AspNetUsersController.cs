using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_capture.Models;
using Data_capture.DataCapture_helper;
using Microsoft.AspNetCore.Http;

namespace Data_capture
{
    public class AspNetUsersController : Controller
    {
        private readonly DataCaptureContext _context;

        public AspNetUsersController(DataCaptureContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PasswordHash,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {

                aspNetUser.Id = new CustomRandom().get36();

                _context.Add(aspNetUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUser);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("PasswordHash,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {

                string tempHash = new Security().ComputeSha256Hash(aspNetUser.PasswordHash);

                AspNetUser getUser = await _context.AspNetUsers
                       .FirstOrDefaultAsync(m => m.UserName.Equals(aspNetUser.UserName) & m.PasswordHash.Equals(aspNetUser.PasswordHash));

                if (getUser == null)
                {
                    ViewBag.userNotFound = "You have entered an invalid username or password";
                    return Login();
                }

                HttpContext.Session.SetString("currentClientId", getUser.Id);//storing PK of user
                
                return RedirectToAction("index", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        // GET: AspNetUsers/Details/5
        public async Task<IActionResult> Details()
        {
            string? id = HttpContext.Session.GetString("currentClientId");

            if (id == null)
            {
                return RedirectToAction("login");
            }

            var aspNetUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: AspNetUsers/Edit/5
        ////public async Task<IActionResult> Edit(string id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var aspNetUser = await _context.AspNetUsers.FindAsync(id);
        ////    if (aspNetUser == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    return View(aspNetUser);
        ////}

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Edit(string id, [Bind("Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        ////{
        ////    if (id != aspNetUser.Id)
        ////    {
        ////        return NotFound();
        ////    }

        ////    if (ModelState.IsValid)
        ////    {
        ////        try
        ////        {
        ////            _context.Update(aspNetUser);
        ////            await _context.SaveChangesAsync();
        ////        }
        ////        catch (DbUpdateConcurrencyException)
        ////        {
        ////            if (!AspNetUserExists(aspNetUser.Id))
        ////            {
        ////                return NotFound();
        ////            }
        ////            else
        ////            {
        ////                throw;
        ////            }
        ////        }
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    return View(aspNetUser);
        ////}

        // GET: AspNetUsers/Delete/5
        ////public async Task<IActionResult> Delete(string id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var aspNetUser = await _context.AspNetUsers
        ////        .FirstOrDefaultAsync(m => m.Id == id);
        ////    if (aspNetUser == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    return View(aspNetUser);
        ////}

        // POST: AspNetUsers/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> DeleteConfirmed(string id)
        ////{
        ////    var aspNetUser = await _context.AspNetUsers.FindAsync(id);
        ////    _context.AspNetUsers.Remove(aspNetUser);
        ////    await _context.SaveChangesAsync();
        ////    return RedirectToAction(nameof(Index));
        ////}

        ////private bool AspNetUserExists(string id)
        ////{
        ////    return _context.AspNetUsers.Any(e => e.Id == id);
        ////}
    }
}
