using fanaticEdit.Data;
using fanaticEdit.DTO;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class AlbumTracksController : Controller
{
    private readonly FanaticServeContext _context;
    private readonly ILogger<AlbumTracksController> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">Database Context</param>
    /// <param name="logger">Logger</param>
    public AlbumTracksController(FanaticServeContext context, ILogger<AlbumTracksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 日本標準時を取得
    /// </summary>
    private DateTime GetJapanTime()
    {
        TimeZoneInfo jst = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        return TimeZoneInfo.ConvertTime(DateTime.Now, jst);
    }

    /// <summary>
    /// Songの一覧をSelectListで取得する
    /// </summary>
    private SelectList GetSongs()
    {
        var items = _context.Songs
            .OrderBy(s => s.Kana)
            .Select(s => new SelectListItem
            {
                Text = s.Title,
                Value = s.SongId.ToString()
            })
            .ToList();

        return new SelectList(items, "Value", "Text");
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id.HasValue)
        {
            try
            {
                // ViewDataにSongListをセット
                ViewData["SongList"] = GetSongs();

                var album = await _context.Albums
                    .Where(m => m.AlbumId == id)
                    .SingleOrDefaultAsync();

                var tracks = await _context.Tracks
                    .Where(m => m.AlbumId == id)
                    .OrderBy(m => m.TrackNo)
                    .ToListAsync();

                return View(new AlbumTracks() { Album = album, Tracks = tracks });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AlbumTracks Edit GET エラー");
                return StatusCode(500, "エラーが発生しました");
            }
        }

        return new EmptyResult();
    }

    [HttpPost]
    public IActionResult AddLine(Guid id, AlbumTracks model)
    {
        try
        {
            if (model != null)
            {
                model.Tracks ??= new List<Track>();
                var count = model.Tracks.Count + 1;
                var japanTime = GetJapanTime();

                model.Tracks.Add(
                    new Track()
                    {
                        TrackId = Guid.NewGuid(),
                        TrackNo = count,
                        CreatedAt = japanTime,
                        ModifiedAt = japanTime,
                        AlbumId = model.Album.AlbumId
                    }
                );
            }

            // ViewDataにSongListをセット
            ViewData["SongList"] = GetSongs();

            return View("Edit", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AlbumTracks AddLine エラー");
            return StatusCode(500, "エラーが発生しました");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, AlbumTracks model)
    {
        try
        {
            if (!ModelState.IsValid && model.Tracks != null)
            {
                ViewData["SongList"] = GetSongs();
                return View(model);
            }

            var japanTime = GetJapanTime();

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
                        existingTrack.ModifiedAt = japanTime;
                    }
                }
                else
                {
                    // 存在しない場合は新規追加
                    _context.Tracks.Add(track);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AlbumTracks Edit POST エラー");
            ModelState.AddModelError("", "処理中にエラーが発生しました");
            
            ViewData["SongList"] = GetSongs();
            return View(model);
        }
    }
}
