using fanaticEdit.Data;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class MediaController : Controller
{
    private readonly FanaticServeContext _context;

    public MediaController(FanaticServeContext context)
    {
        _context = context;
    }

    // GET: Media
    public async Task<IActionResult> Index()
    {
        // var arr = _context.Media;
        var arr = _context.Media.Select(r => new Medium() { MediaType = r.MediaType, Name = r.Name, CreatedAt = r.CreatedAt, ModifiedAt = r.ModifiedAt });

        return View(await arr.ToArrayAsync());
    }

    // GET: Media/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Media/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MediaType,Name,CreatedAt,ModifiedAt")] Medium medium)
    {
        if (ModelState.IsValid)
        {
            _context.Add(medium);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(medium);
    }

    // GET: Media/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // var medium = await _context.Media.OrderBy(r => r.MediaType).Select(r => new Medium { Name = r.Name, MediaType = r.MediaType }).ToListAsync();
        var medium = await _context.Media.Select(r => new Medium { Name = r.Name, MediaType = r.MediaType }).FirstOrDefaultAsync(r=>r.MediaType == id);

        if (medium == null)
        {
            return NotFound();
        }
        return View(medium);
    }

    // POST: Media/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MediaType,Name,CreatedAt,ModifiedAt")] Medium medium)
    {
        if (id != medium.MediaType)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(medium);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediumExists(medium.MediaType))
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
        return View(medium);
    }

    // GET: Media/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var medium = await _context.Media
            .FirstOrDefaultAsync(m => m.MediaType == id);
        if (medium == null)
        {
            return NotFound();
        }

        return View(medium);
    }

    // POST: Media/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var medium = await _context.Media.FindAsync(id);
        if (medium != null)
        {
            _context.Media.Remove(medium);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MediumExists(int id)
    {
        return _context.Media.Any(e => e.MediaType == id);
    }
}
