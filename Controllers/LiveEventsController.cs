using fanaticEdit.Data;
using fanaticEdit.Enum;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace fanaticEdit.Controllers;

public class LiveEventsController : Controller
{
    private readonly FanaticServeContext _context;

    public LiveEventsController(FanaticServeContext context)
    {
        _context = context;
    }

    // GET: LiveEvents
    public async Task<IActionResult> Index()
    {
        var le = await _context.LiveEvents
            .GroupJoin(_context.LiveEventNotes,
                       liveEvent => liveEvent.LiveEventId,
                       note => note.LiveEventId,
                       (liveEvent, notes) => new { LiveEvent = liveEvent, Notes = notes })
            .Select(group => new LiveEvent
            {
                LiveEventId = group.LiveEvent.LiveEventId,
                Title = group.LiveEvent.Title,
                Place = group.LiveEvent.Place,
                PerformAt = group.LiveEvent.PerformAt,
                CreatedAt = group.LiveEvent.CreatedAt,
                ModifiedAt = group.LiveEvent.ModifiedAt,
                LiveEventNote = group.Notes.FirstOrDefault() ?? new LiveEventNote() { LiveEventId = group.LiveEvent.LiveEventId, }
            })
            .OrderBy(i => i.PerformAt).ToListAsync();

        return View(le);
    }

    // GET: LiveEvents/Create
    public IActionResult Create()
    {
        var model = new LiveEvent()
        {
            LiveEventId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        return View(model);
    }

    // POST: LiveEvents/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LiveEventId,Title,Place,PerformAt,CreatedAt,ModifiedAt,AbstractEventId")] LiveEvent liveEvent, string? urlsData)
    {
        if (ModelState.IsValid)
        {
            _context.Add(liveEvent);
            await _context.SaveChangesAsync();

            if (liveEvent.AbstractEventId.HasValue)
            {
                // 抽象イベントIDが指定されている場合、Linkを設定する
                var abstractEventLink = new AbstractEventLink()
                {
                    AbstractEventId = liveEvent.AbstractEventId.Value,
                    EventId = liveEvent.LiveEventId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
                _context.AbstractEventLinks.Add(abstractEventLink);
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(urlsData))
            {
                try
                {
                    var urlsList = JsonSerializer.Deserialize<List<dynamic>>(urlsData);
                    if (urlsList != null)
                    {
                        var timeStamp = DateTime.Now;
                        foreach (var urlItem in urlsList)
                        {
                            if (!string.IsNullOrEmpty(urlItem.GetProperty("url").GetString()))
                            {
                                Live_Event_Url leu = new Live_Event_Url()
                                {
                                    Live_Event_Id = liveEvent.LiveEventId,  // ✅ 追加
                                    Url = urlItem.GetProperty("url").GetString(),
                                    Description = urlItem.GetProperty("description").GetString(),
                                    Created_At = timeStamp,
                                    Modified_At = timeStamp
                                };
                                _context.LiveEventUrls.Add(leu);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch { }
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: LiveEvents/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var le = await _context.LiveEvents
            .Where(le => le.LiveEventId == id)
            .GroupJoin(_context.LiveEventNotes,
                       liveEvent => liveEvent.LiveEventId,
                       note => note.LiveEventId,
                       (liveEvent, notes) => new { LiveEvent = liveEvent, Notes = notes })
            .Select(group => new LiveEvent
            {
                LiveEventId = group.LiveEvent.LiveEventId,
                Title = group.LiveEvent.Title,
                Place = group.LiveEvent.Place,
                PerformAt = group.LiveEvent.PerformAt,
                CreatedAt = group.LiveEvent.CreatedAt,
                ModifiedAt = group.LiveEvent.ModifiedAt,
                LiveEventNote = group.Notes.FirstOrDefault() ?? new LiveEventNote() { LiveEventId = group.LiveEvent.LiveEventId, }
            })
            .FirstOrDefaultAsync();

        if (le == null)
        {
            return NotFound();
        }

        var absEventId = await _context.AbstractEventLinks.FirstOrDefaultAsync(w => w.EventId == id);
        if (absEventId != null)
        {
            le.AbstractEventId = absEventId.AbstractEventId;
        }

        le.SetList = await _context.SetLists
            .Where(l => l.LiveEventId == id)
            .GroupJoin(
            _context.SetListNotes,
            (SetList) => SetList.SetListId,
            (Note) => Note.SetListId,
            (SetList, Note) => new { SetList, Note }
            )
            .Select(group => new SetList()
            {
                LiveEventId = group.SetList.LiveEventId,
                SetListId = group.SetList.SetListId,
                SetListNo = group.SetList.SetListNo,
                SongId = group.SetList.SongId,
                Title = group.SetList.Title,
                Singing = group.SetList.Singing,
                Medley = group.SetList.Medley,
                Cover = group.SetList.Cover,
                Part_Type = group.SetList.Part_Type,
                CreatedAt = group.SetList.CreatedAt,
                ModifiedAt = group.SetList.ModifiedAt,
                Note = group.Note.FirstOrDefault(),
            }
            )
            .OrderBy(r => r.SetListNo).ToListAsync();

        // Event_list_urlを取得する
        var liveEventUrls =await _context.LiveEventUrls
            .Where(leu => leu.Live_Event_Id == id)
            .OrderBy(leu=>leu.Live_Event_Url_Id)
            .ToListAsync();

        // JSON 化して ViewBag に格納
        ViewBag.UrlsData = JsonSerializer.Serialize(liveEventUrls.Select(u => new
        {
            url = u.Url,
            description = u.Description
        }));
        return View(le);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddNewSetList(Guid id, [Bind("LiveEventId,Title,Place,PerformAt,CreatedAt,ModifiedAt,LiveEventNote,SetList,AbstractEventId")] LiveEvent liveEvent)
    {
        liveEvent.SetList ??= new List<SetList>();
        liveEvent.SetList = liveEvent.SetList.OrderBy(sl => sl.SetListNo).ToList();

        int lastNo = (liveEvent.SetList.Any() ? liveEvent.SetList.Max(m => m.SetListNo) : 0) + 1;

        // 先頭に追加
        liveEvent.SetList.Insert(0, new SetList() { LiveEventId = id, SetListId = Guid.NewGuid(), SetListNo = lastNo, Part_Type = Part.Main.ToInt() });

        // インデックスをリセット：ModelState を消去してバインディングを再構築
        ModelState.Clear();

        return View("Edit", liveEvent);
    }

    // POST: LiveEvents/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("LiveEventId,Title,Place,PerformAt,CreatedAt,ModifiedAt,AbstractEventId,LiveEventNote,SetList")] LiveEvent liveEvent, string? urlsData)
    {
        // ライブイベントのレコードが存在するか
        if (id != liveEvent.LiveEventId)
        {
            return NotFound();
        }

        try
        {
            var timeStamp = DateTime.Now;
            liveEvent.ModifiedAt = timeStamp;
            _context.Update(liveEvent);

            // Eventの変更をDBに反映する
            await _context.SaveChangesAsync();

            // AbstractEvetIdの変更をDBに反映する

            if (liveEvent.AbstractEventId.HasValue)
            {
                // 入力されたAbstractEvetId がある

                var records = _context.AbstractEventLinks.Where(w => w.EventId == liveEvent.LiveEventId);
                // DBにレコードがあれば更新
                if (records != null && records.Any())
                {
                    foreach (var rec in records)
                    {
                        rec.AbstractEventId = liveEvent.AbstractEventId.Value;
                    }

                    _context.AbstractEventLinks.UpdateRange(records);
                }
                else
                {
                    // DBにレコードが無ければ追加
                    _context.AbstractEventLinks.Add(new AbstractEventLink()
                    {
                        EventId = liveEvent.LiveEventId,
                        AbstractEventId = liveEvent.AbstractEventId.Value,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                    });
                }
            }
            else
            {
                // 入力されたAbstractEvetId が空

                var records = _context.AbstractEventLinks.Where(w => w.EventId == liveEvent.LiveEventId);
                if (records != null)
                {
                    // DBにレコードがあれば削除
                    _context.AbstractEventLinks.RemoveRange(records);
                }
            }
            _context.SaveChanges();

            // LiveEvent noteの処理
            if (String.IsNullOrEmpty(liveEvent.LiveEventNote.Note))
            {
                // 入力のnoteが空、既存は削除するパターン
                await _context.LiveEventNotes
                                    .Where(m => m.LiveEventId == id)
                                    .ExecuteDeleteAsync();
            }
            else
            {
                // Noteを入力した

                // 既存Noteのレコードが存在するか
                var note = await _context.LiveEventNotes.FindAsync(id);
                if (note == null)
                {
                    // 追加
                    note = liveEvent.LiveEventNote;
                    note.LiveEventId = id;
                    note.CreatedAt = timeStamp;
                    note.ModifiedAt = timeStamp;

                    _context.LiveEventNotes.Add(note);
                }
                else
                {
                    // 更新
                    note.Note = liveEvent.LiveEventNote.Note;
                    note.ModifiedAt = timeStamp;
                }
            }
            // Event Noteの変更をDBに反映する
            await _context.SaveChangesAsync();

            // SetListの処理
            if (liveEvent.SetList != null)
            {
                if (liveEvent.SetList.Any(m => m.WillBeRemove))
                {
                    // 削除対象のNote
                    foreach (var sl in liveEvent.SetList.Where(m => m.WillBeRemove))
                    {
                        await _context.SetListNotes.Where(m => m.SetListId == sl.SetListId).ExecuteDeleteAsync();
                        _context.SetLists.Remove(sl);
                    }
                    // Event Noteの変更をDBに反映する
                    await _context.SaveChangesAsync();
                }

                foreach (var sl in liveEvent.SetList.Where(m => m.WillBeRemove == false))
                {
                    if (await _context.SetLists.AnyAsync(r => r.SetListId == sl.SetListId))
                    {
                        // 更新した
                        sl.ModifiedAt = timeStamp;
                        sl.Singing = sl.Singing;
                        sl.Medley = sl.Medley;
                        sl.Cover = sl.Cover;
                        sl.Part_Type = sl.Part_Type;
                        _context.SetLists.Update(sl);

                        if (sl.Note != null)
                        {
                            if (String.IsNullOrEmpty(sl.Note.Note))
                            {
                                // 既存ノートがあれば削除
                                await _context.SetListNotes.Where(m => m.SetListId == sl.SetListId).ExecuteDeleteAsync();
                            }
                            else
                            {
                                // 更新 or 追加
                                var listNote = await _context.SetListNotes.FindAsync(sl.SetListId);
                                if (listNote != null)
                                {
                                    listNote.Note = sl.Note.Note;
                                    listNote.ModifiedAt = timeStamp;
                                }
                                else
                                {
                                    listNote = new SetListNote() { SetListId = sl.SetListId, Note = sl.Note.Note, CreatedAt = timeStamp, ModifiedAt = timeStamp };
                                    await _context.SetListNotes.AddAsync(listNote);
                                }
                            }
                        }
                        continue;
                    }
                    else
                    {
                        // 追加した
                        sl.LiveEventId = id;
                        sl.ModifiedAt = timeStamp;
                        sl.CreatedAt = timeStamp;
                        await _context.SetLists.AddAsync(sl);

                        // SetListのNoteの処理
                        if (sl.Note != null && !String.IsNullOrEmpty(sl.Note.Note))
                        {
                            sl.Note.SetListId = sl.SetListId;
                            sl.Note.ModifiedAt = timeStamp;
                            sl.Note.CreatedAt = timeStamp;
                            await _context.SetListNotes.AddAsync(sl.Note);
                        }
                    }
                }
            }
            // EFを通して変更をDBに反映する
            await _context.SaveChangesAsync();

            // Live_Event_Url の処理 - 既存URLをすべて削除して新しいものを追加
            await _context.LiveEventUrls.Where(m => m.Live_Event_Id == id).ExecuteDeleteAsync();

            if (!string.IsNullOrEmpty(urlsData))
            {
                try
                {
                    var urlsList = JsonSerializer.Deserialize<List<dynamic>>(urlsData);
                    if (urlsList != null)
                    {
                        foreach (var urlItem in urlsList)
                        {
                            if (!string.IsNullOrEmpty(urlItem.GetProperty("url").GetString()))
                            {
                                Live_Event_Url leu = new Live_Event_Url()
                                {
                                    Live_Event_Id = id,  // ✅ 外部キーを正しく設定
                                    Url = urlItem.GetProperty("url").GetString(),
                                    Description = urlItem.GetProperty("description").GetString(),
                                    Created_At = timeStamp,
                                    Modified_At = timeStamp
                                };
                                _context.LiveEventUrls.Add(leu);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    // ログに記録またはユーザーに通知
                    Console.WriteLine($"URL処理エラー: {ex.Message}");
                }
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LiveEventExists(liveEvent.LiveEventId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction("Edit", id);
    }

    // GET: LiveEvents/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var liveEvent = await _context.LiveEvents
            .FirstOrDefaultAsync(m => m.LiveEventId == id);
        if (liveEvent == null)
        {
            return NotFound();
        }

        return View(liveEvent);
    }

    // POST: LiveEvents/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            // 関連データをカスケード削除
            await _context.SetListNotes
                .Where(n => _context.SetLists
                    .Where(sl => sl.LiveEventId == id)
                    .Select(sl => sl.SetListId)
                    .Contains(n.SetListId))
                .ExecuteDeleteAsync();

            await _context.SetLists
                .Where(sl => sl.LiveEventId == id)
                .ExecuteDeleteAsync();

            await _context.LiveEventUrls
                .Where(u => u.Live_Event_Id == id)
                .ExecuteDeleteAsync();

            await _context.LiveEventNotes
                .Where(n => n.LiveEventId == id)
                .ExecuteDeleteAsync();

            await _context.AbstractEventLinks
                .Where(al => al.EventId == id)
                .ExecuteDeleteAsync();

            var liveEvent = await _context.LiveEvents.FindAsync(id);
            if (liveEvent != null)
            {
                _context.LiveEvents.Remove(liveEvent);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // ログに記録
            Console.WriteLine($"LiveEvent 削除エラー: {ex.Message}");
            return StatusCode(500, "LiveEvent 削除処理中にエラーが発生しました");
        }

        return RedirectToAction(nameof(Index));
    }

    private bool LiveEventExists(Guid id)
    {
        return _context.LiveEvents.Any(e => e.LiveEventId == id);
    }

    // GET: LiveEvents/SongSelector
    public async Task<IActionResult> SongSelector()
    {
        var songs = await _context.Songs
            .OrderBy(s => s.Title)
            .ToListAsync();

        return View(songs);
    }
}
