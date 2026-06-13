using fanaticEdit.Data;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class SongsController : Controller
{
    private readonly FanaticServeContext _context;

    public SongsController(FanaticServeContext context)
    {
        _context = context;
    }

    // GET: Songs
    public async Task<IActionResult> Index()
    {
        return View(await _context.Songs.OrderBy(r => r.Kana).ToListAsync());
    }

    // GET: Songs/Create
    public IActionResult Create()
    {
        return View(new Song() { SongId = Guid.NewGuid(), CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now });
    }

    // POST: Songs/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("SongId,Title,Kana,CreatedAt,ModifiedAt")] Song song)
    {
        if (ModelState.IsValid)
        {
            song.SongId = Guid.NewGuid();
            _context.Add(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(song);
    }

    // GET: Songs/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var song = await _context.Songs.FindAsync(id);
        if (song == null)
        {
            return NotFound();
        }

        var note =await _context.SongNotes.FindAsync(id);
        song.Note = note?.Note;

        return View(song);
    }

    // POST: Songs/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("SongId,Title,Kana,CreatedAt,ModifiedAt,Note")] Song song)
    {
        if (id != song.SongId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(song);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(song.SongId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (String.IsNullOrEmpty(song.Note)) {
                await _context.SongNotes
                                   .Where(note => note.SongId == id)
                                   .ExecuteDeleteAsync();
            }
            else { 
              // idがSongNotesテーブルに存在する場合は更新、無ければ追加する
                var songNote = await _context.SongNotes.FirstOrDefaultAsync(n => n.SongId == id);
                if (songNote != null)
                {
                    songNote.Note = song.Note;
                    songNote.ModifiedAt = DateTime.Now;
                    _context.SongNotes.Update(songNote);
                }
                else
                {
                    _context.SongNotes.Add(new SongNote
                    {
                        SongId = id,
                        Note = song.Note,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();
                
            }

            return RedirectToAction(nameof(Index));
        }
        return View(song);
    }

    // GET: Songs/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var song = await _context.Songs
            .FirstOrDefaultAsync(m => m.SongId == id);
        if (song == null)
        {
            return NotFound();
        }

        return View(song);
    }

    // POST: Songs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song != null)
        {
            _context.Songs.Remove(song);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SongExists(Guid id)
    {
        return _context.Songs.Any(e => e.SongId == id);
    }
}
