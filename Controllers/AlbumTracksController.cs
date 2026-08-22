using fanaticEdit.Data;
using fanaticEdit.DTO;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class AlbumTracksController : Controller
{

    private readonly FanaticServeContext _context;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">Database Context</param>
    public AlbumTracksController(FanaticServeContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id.HasValue)
        {
            var album = await (_context.Albums
                .Where(m => m.AlbumId == id)
                .SingleOrDefaultAsync());

            var tracks = await (_context.Tracks
                .Where(m => m.AlbumId == id)
                .OrderBy(m => m.TrackNo)
                ).ToListAsync();
            return View(
                new AlbumTracks() { Album = album, Tracks = tracks }
                );
        }

        return new EmptyResult();
    }

    [HttpPost]
    public IActionResult AddLine(Guid id, AlbumTracks model)
    {

        if (model != null)
        {
            model.Tracks ??= new List<Track>();
            var count = model.Tracks.Count + 1;
            model.Tracks.Add(
                new Track()
                {
                    TrackId = Guid.NewGuid(),
                    TrackNo = count,

                    // JCTで作成日時と更新日時を設定する
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    AlbumId = model.Album.AlbumId
                }
            );
            return View("Edit", model);
        }
        return new EmptyResult();
    }


    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, AlbumTracks model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        foreach (var track in model.Tracks)
        {
            // trackが Tracksテーブルに存在するか?
            if (_context.Tracks.Any(t => t.TrackId == track.TrackId))
            {
                // 存在する場合は更新
                var existingTrack = await _context.Tracks.FindAsync(track.TrackId);
                if (existingTrack != null)
                {
                    existingTrack.TrackNo = track.TrackNo;
                    existingTrack.Title = track.Title;
                    existingTrack.Length = track.Length;
                    existingTrack.SongId = track.SongId;

                    // JCTで作成日時と更新日時を設定する
                    existingTrack.ModifiedAt = DateTime.Now;
                }
            }
            else
            {
                // 存在しない場合は新規追加
                _context.Tracks.Add(track);
            }
        }
        _context.SaveChanges();

        return View(model);
    }

}
