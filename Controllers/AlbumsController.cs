using fanaticEdit.Data;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class AlbumsController : Controller
{
    private readonly FanaticServeContext _context;
    private readonly ILogger<AlbumsController> _logger; // ILoggerを追加

    public AlbumsController(FanaticServeContext context, ILogger<AlbumsController> logger)
    {
        _context = context;
        _logger = logger; // ILoggerのインスタンスを取得
    }

    // GET: Albums
    public async Task<IActionResult> Index()
    {
        return View(await _context.Albums.OrderBy(record => record.Title).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        var model = new Models.Album()
        {
            AlbumId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };

        // 非同期メソッドの結果を正しく取得して代入
        model.MeidaTypeOptions = await _context.Media.Select(r => new Medium() { MediaType = r.MediaType, Name = r.Name }).ToListAsync();
        return View(model);
    }

    // POST: Albums/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AlbumId,Code,Title,MediaType,ReleaseOn,LabelId,CreatedAt,ModifiedAt,AbstractAlbumID")] Album album)
    {
        if (ModelState.IsValid)
        {
            if (album.AbstractAlbumID != null)
            {
                var AbstractAlbumLink = new AbstractAlbumLink();
                AbstractAlbumLink.AlbumId = album.AlbumId;
                AbstractAlbumLink.AbstractAlbumId = album.AbstractAlbumID.Value;
                AbstractAlbumLink.CreatedAt = DateTime.Now;
                AbstractAlbumLink.ModifiedAt = DateTime.Now;

                _context.Add(AbstractAlbumLink);
            }

            _context.Add(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(album);
    }

    // GET: Albums/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // ViewDataにLabelマスタをセット
        ViewData["Label"] = GetLabels();

        var album = await _context.Albums.FindAsync(id);
        if (album == null)
        {
            return NotFound();
        }

        var note = await _context.AlbumNotes.FindAsync(id);
        if (note != null)
        {
            album.Note = note.Note;
        }

        album.MeidaTypeOptions = await _context.Media.Select(r => new Medium() { MediaType = r.MediaType, Name = r.Name }).ToListAsync();
        var link = await _context.AbstractAlbumLinks.SingleOrDefaultAsync(r => r.AlbumId == id);
        album.AbstractAlbumID = link?.AbstractAlbumId;

        return View(album);
    }

    // POST: Albums/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("AlbumId,Code,Title,MediaType,ReleaseOn,LabelId,CreatedAt,ModifiedAt,AbstractAlbumID,Note")] Album album)
    {
        if (id != album.AlbumId)
        {
            return NotFound();
        }

        // ViewDataにLabelマスタをセット
        ViewData["Label"] = GetLabels();

        if (ModelState.IsValid)
        {
            try
            {
                album.ModifiedAt = DateTime.Now;
                _context.Update(album);

                if (album.AbstractAlbumID != null)
                {
                    AbstractAlbumLink link;
                    try
                    {
                        link = await _context.AbstractAlbumLinks.SingleAsync(r => r.AlbumId == album.AlbumId);
                        link.AbstractAlbumId = album.AbstractAlbumID.Value;
                        link.ModifiedAt = DateTime.Now;
                        _context.Update(link);

                    }
                    catch
                    {
                        // 新規
                        link = new AbstractAlbumLink() { AlbumId = album.AlbumId, CreatedAt = DateTime.Now };
                        link.AbstractAlbumId = album.AbstractAlbumID.Value;
                        link.ModifiedAt = DateTime.Now;
                        _context.AbstractAlbumLinks.Add(link);
                    }
                }

                // Noteの更新
                if (string.IsNullOrEmpty(album.Note))
                {
                    // 空欄 既存レコード削除
                    await (
                        _context.AlbumNotes
                        .Where(note => note.AlbumId == id)
                        .ExecuteDeleteAsync()
                    );
                }
                else
                {
                    // id がAlbumNotesテーブルにあれば更新、無ければ追加する
                    var albumNote = await _context.AlbumNotes.SingleOrDefaultAsync(note => note.AlbumId == id);
                    if (albumNote != null)
                    {
                        // 既存レコードがあれば更新
                        albumNote.Note = album.Note;
                        albumNote.ModifiedAt = DateTime.Now;
                        _context.AlbumNotes.Update(albumNote);
                    }
                    else
                    {
                        // 無ければ追加
                        var newNote = new AlbumNote
                        {
                            AlbumId = id,
                            Note = album.Note,
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        };
                        _context.AlbumNotes.Add(newNote);
                    }

                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(album.AlbumId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // ロギング
                _logger.LogError(ex, "Album処理エラー");
                // または
                ModelState.AddModelError("", "Albumの処理に失敗しました");
            }
            return RedirectToAction(nameof(Index));
        }
        return View(album);
    }

    // GET: Albums/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albums
            .FirstOrDefaultAsync(m => m.AlbumId == id);
        if (album == null)
        {
            return NotFound();
        }

        return View(album);
    }

    // POST: Albums/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album != null)
        {
            _context.Albums.Remove(album);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AlbumExists(Guid id)
    {
        return _context.Albums.Any(e => e.AlbumId == id);
    }


    // LabelのListを取得する
    private SelectList GetLabels()
    {
        var items =
             _context.Labels.OrderBy(r => r.Name)
            .Select(l => new SelectListItem
            {
                Text = l.Name,
                Value = l.LabelId.ToString()
            }
            ).ToList()
            ;

        return new SelectList(items, "Value", "Text");
    }
}
