using fanaticEdit.Data;
using fanaticEdit.DTO;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace fanaticEdit.Controllers;

public class RoleOnSongController : Controller
{
    private readonly FanaticServeContext _context;

    public RoleOnSongController(FanaticServeContext context)
    {
        _context = context;
    }

    // GET: RoleOnSongController/Edit/5
    public ActionResult Edit(Guid? id)
    {
        // idがnullの場合はBadRequestを返す
        if (id == null)
        {
            return BadRequest();
        }

        // ViewDataにLabelマスタをセット
        ViewData["RoleList"] = new RoleService(_context).GetRoles();

        EditRoleOnSong model = new EditRoleOnSong();

        model.song =
            _context.Songs
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

        _context.RoleOnSongs
            .Where(r => r.SongId == id)
            .ToList()
            .ForEach(pwr =>
            {
                var personWithRole = new PersonWithRole() { personWithRoleId = pwr.Id };
                var person =
                _context.People.Where(p => p.PersonId == pwr.PersonId)
                    .FirstOrDefault();
                personWithRole.person = person;

                personWithRole.role =
                _context.Roles.Where(r => r.RoleId == pwr.RoleId)
                .FirstOrDefault();

                personWithRole.roleName = personWithRole.role != null ? personWithRole.role.Name : "";

                model.personWithRoles.Add(personWithRole);
            }
            );

        // 曲の詳細と紐づくロール-人物データの一覧を編集画面に表示する
        return View(model);
    }

    // POST: RoleOnSongController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Guid? id, [Bind("song, personWithRoles")] EditRoleOnSong model)
    {
        if (id == null)
        {
            return BadRequest();
        }

        foreach (var pwr in model.personWithRoles)
        {
            // 既存のroleOnSongデータがあれば更新、なければ追加する

            var existingRoleOnSong = _context.RoleOnSongs
                .Where(r => r.Id == pwr.personWithRoleId)
                .FirstOrDefault();
            if (existingRoleOnSong == null)
            {
                // RoleOnSongレコードの追加
                var newRoleOnSong = new RoleOnSong()
                {
                    SongId = model.song.SongId,
                    PersonId = pwr.person.PersonId,
                    RoleId = pwr.role.RoleId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
                _context.RoleOnSongs.Add(newRoleOnSong);
            }
            else
            {
                // RoleOnSongレコードの更新
                existingRoleOnSong.RoleId = pwr.role.RoleId;
                existingRoleOnSong.PersonId = pwr.person.PersonId;
                existingRoleOnSong.ModifiedAt = DateTime.Now;
            }
        }
        _context.SaveChanges();

        return RedirectToAction("Index", "Songs");
    }

    // POST: RoleOnSongController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }



}
