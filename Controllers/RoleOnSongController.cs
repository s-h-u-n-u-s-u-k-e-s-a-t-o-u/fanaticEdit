using fanaticEdit.Data;
using fanaticEdit.DTO;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace fanaticEdit.Controllers;

public class RoleOnSongController : Controller
{
    private readonly FanaticServeContext _context;
    private readonly ILogger<RoleOnSongController> _logger;

    public RoleOnSongController(FanaticServeContext context, ILogger<RoleOnSongController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: RoleOnSong/Edit/5
    public ActionResult Edit(Guid? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            // ViewDataにRoleマスタをセット（SelectListItemのリストに統一）
            var roles = _context.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.Name
                })
                .ToList();
            ViewData["RoleList"] = roles;

            EditRoleOnSong model = new EditRoleOnSong();

            model.song = _context.Songs
                .Where(s => s.SongId == id)
                .Select(s => new Song()
                {
                    SongId = s.SongId,
                    Title = s.Title,
                    Kana = s.Kana,
                    CreatedAt = s.CreatedAt,
                    ModifiedAt = s.ModifiedAt
                })
                .FirstOrDefault();

            if (model.song == null)
            {
                return NotFound();
            }

            _context.RoleOnSongs
                .Where(r => r.SongId == id)
                .OrderBy(r => r.CreatedAt)
                .ToList()
                .ForEach(pwr =>
                {
                    var personWithRole = new PersonWithRole() { personWithRoleId = pwr.Id };
                    var person = _context.People.FirstOrDefault(p => p.PersonId == pwr.PersonId);
                    personWithRole.person = person;

                    personWithRole.role = _context.Roles.FirstOrDefault(r => r.RoleId == pwr.RoleId);
                    personWithRole.roleName = personWithRole.role?.Name ?? "";

                    model.personWithRoles.Add(personWithRole);
                });

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RoleOnSong Edit GET エラー");
            return StatusCode(500, "エラーが発生しました");
        }
    }

    // POST: RoleOnSong/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Guid? id, [Bind("song,personWithRoles")] EditRoleOnSong model)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            var timeStamp = DateTime.UtcNow;

            // 新規追加されたレコードを追跡
            var newRecords = new List<RoleOnSong>();
            var updateRecords = new List<RoleOnSong>();

            foreach (var pwr in model.personWithRoles)
            {
                // 必須フィールドの検証
                if (pwr.person?.PersonId == Guid.Empty || pwr.role?.RoleId <= 0)
                {
                    _logger.LogWarning($"スキップ: PersonId={pwr.person?.PersonId}, RoleId={pwr.role?.RoleId}");
                    continue;
                }

                var existingRoleOnSong = _context.RoleOnSongs
                    .FirstOrDefault(r => r.Id == pwr.personWithRoleId && pwr.personWithRoleId != 0);

                if (existingRoleOnSong == null)
                {
                    // 重複チェック（同じSongIdとPersonIdの組み合わせが存在するかチェック）
                    var duplicate = _context.RoleOnSongs
                        .Any(r => r.SongId == model.song.SongId && r.PersonId == pwr.person.PersonId);

                    if (!duplicate)
                    {
                        var newRoleOnSong = new RoleOnSong()
                        {
                            SongId = model.song.SongId,
                            PersonId = pwr.person.PersonId,
                            RoleId = pwr.role.RoleId,
                            CreatedAt = timeStamp,
                            ModifiedAt = timeStamp
                        };
                        newRecords.Add(newRoleOnSong);
                        _logger.LogInformation($"新規追加: SongId={model.song.SongId}, PersonId={pwr.person.PersonId}, RoleId={pwr.role.RoleId}");
                    }
                }
                else
                {
                    // RoleOnSongレコードの更新
                    existingRoleOnSong.RoleId = pwr.role.RoleId;
                    existingRoleOnSong.PersonId = pwr.person.PersonId;
                    existingRoleOnSong.ModifiedAt = timeStamp;
                    updateRecords.Add(existingRoleOnSong);
                    _logger.LogInformation($"更新: Id={existingRoleOnSong.Id}, RoleId={pwr.role.RoleId}");
                }
            }

            // バッチで追加・更新
            if (newRecords.Count > 0)
            {
                _context.RoleOnSongs.AddRange(newRecords);
            }

            if (updateRecords.Count > 0)
            {
                _context.RoleOnSongs.UpdateRange(updateRecords);
            }

            var saveCount = _context.SaveChanges();
            _logger.LogInformation($"保存完了: {saveCount}件");

            return RedirectToAction("Index", "Songs");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RoleOnSong Edit POST エラー");
            ModelState.AddModelError("", $"処理中にエラーが発生しました: {ex.Message}");
            
            SetViewData();
            return View(model);
        }
    }

    // GET: RoleOnSong/PersonSelector
    public IActionResult PersonSelector()
    {
        var persons = _context.People
            .OrderBy(p => p.Kana)
            .ToList();

        return View(persons);
    }

    /// <summary>
    /// ViewDataを設定するヘルパーメソッド
    /// </summary>
    private void SetViewData()
    {
        var roleService = new RoleService(_context);
        var roles = roleService.GetRoles();

        // SelectList なら IEnumerable<SelectListItem> にキャストして ToList()、
        // そうでなければ List<SelectListItem> か新規リスト
        if (roles is IEnumerable<SelectListItem> selectListItems)
        {
            ViewData["RoleList"] = selectListItems.ToList();
        }
        else
        {
            ViewData["RoleList"] = new List<SelectListItem>();
        }
    }
}
