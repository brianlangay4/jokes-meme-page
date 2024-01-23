using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesSquad.Data;
using JokesSquad.Models;
using Microsoft.AspNetCore.Authorization;

namespace JokesSquad.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jokes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jokes.ToListAsync());
        }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokes = await _context.Jokes
                .FirstOrDefaultAsync(m => m.id == id);
            if (jokes == null)
            {
                return NotFound();
            }

            return View(jokes);
        }

        [Authorize]
        // GET: Jokes/Create
        public IActionResult Create()
        {
            return View();
        }
        // GET: Jokes/show search form
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // POST: Jokes/show search results
        public async Task<IActionResult> ShowSearchResult(String SearchPhrase)
        {
            return View("index", await _context.Jokes.Where(J=>J.JokesQuestion.Contains(SearchPhrase)).ToListAsync());
        }

        /*    public String ShowSearchResult(String SearchPhrase)
            {
                return "Your looking for " + SearchPhrase;
            }*/


        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //****************important******
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,JokesQuestion,JokesAnswer")] Jokes jokes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jokes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jokes);
        }

        //****************important******
        [Authorize]

        // GET: Jokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokes = await _context.Jokes.FindAsync(id);
            if (jokes == null)
            {
                return NotFound();
            }
            return View(jokes);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //****************important******
        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,JokesQuestion,JokesAnswer")] Jokes jokes)
        {
            if (id != jokes.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jokes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokesExists(jokes.id))
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
            return View(jokes);
        }

     
        // GET: Jokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokes = await _context.Jokes
                .FirstOrDefaultAsync(m => m.id == id);
            if (jokes == null)
            {
                return NotFound();
            }

            return View(jokes);
        }

        // POST: Jokes/Delete/5
        //****************important******
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jokes = await _context.Jokes.FindAsync(id);
            if (jokes != null)
            {
                _context.Jokes.Remove(jokes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokesExists(int id)
        {
            return _context.Jokes.Any(e => e.id == id);
        }
    }
}
