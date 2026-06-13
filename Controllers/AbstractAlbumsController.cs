using fanaticEdit.Data;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class AbstractAlbumsController : Controller
{
    private readonly FanaticServeContext _context;

    public AbstractAlbumsController(FanaticServeContext context)
    {
        _context = context;
    }

    // GET: AbstractAlbums
    public async Task<IActionResult> Index()
    {
        return View(await _context.AbstractAlbums.OrderBy(m => m.Title).ToListAsync());
    }

    // GET: AbstractAlbums/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var abstractAlbum = await _context.AbstractAlbums
            .FirstOrDefaultAsync(m => m.AbstractAlbumId == id);
        if (abstractAlbum == null)
        {
            return NotFound();
        }

        return View(abstractAlbum);
    }

    // GET: AbstractAlbums/Create
    public IActionResult Create()
    {
        return View(new AbstractAlbum()
        {
            AbstractAlbumId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        }
            );
    }

    // POST: AbstractAlbums/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AbstractAlbumId,Title,CreatedAt,ModifiedAt")] AbstractAlbum abstractAlbum)
    {
        if (ModelState.IsValid)
        {
            abstractAlbum.AbstractAlbumId = Guid.NewGuid();
            _context.Add(abstractAlbum);

            // 実アルバムも1件作る
            var album =
                new Models.Album()
                {
                    AlbumId = Guid.NewGuid(),
                    Title = abstractAlbum.Title,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ReleaseOn = DateTime.Now
                };
            _context.Albums.Add(album);

            // link
            _context.AbstractAlbumLinks.Add(
                new Models.AbstractAlbumLink()
                {
                    AbstractAlbumId = abstractAlbum.AbstractAlbumId,
                    AlbumId = album.AlbumId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                }
                );

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(abstractAlbum);
    }

    // GET: AbstractAlbums/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var abstractAlbum = await _context.AbstractAlbums.FindAsync(id);
        if (abstractAlbum == null)
        {
            return NotFound();
        }
        return View(abstractAlbum);
    }

    // POST: AbstractAlbums/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("AbstractAlbumId,Title,CreatedAt,ModifiedAt")] AbstractAlbum abstractAlbum)
    {
        if (id != abstractAlbum.AbstractAlbumId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(abstractAlbum);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbstractAlbumExists(abstractAlbum.AbstractAlbumId))
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
        return View(abstractAlbum);
    }

    // GET: AbstractAlbums/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var abstractAlbum = await _context.AbstractAlbums
            .FirstOrDefaultAsync(m => m.AbstractAlbumId == id);
        if (abstractAlbum == null)
        {
            return NotFound();
        }

        return View(abstractAlbum);
    }

    // POST: AbstractAlbums/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var abstractAlbum = await _context.AbstractAlbums.FindAsync(id);
        if (abstractAlbum != null)
        {
            _context.AbstractAlbums.Remove(abstractAlbum);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AbstractAlbumExists(Guid id)
    {
        return _context.AbstractAlbums.Any(e => e.AbstractAlbumId == id);
    }
}
