using fanaticEdit.Data;
using fanaticEdit.DTO;
using Microsoft.AspNetCore.Mvc;

namespace fanaticEdit.Controllers;

public class RoleOnTrackController : Controller
{
    private readonly FanaticServeContext _context;
    public RoleOnTrackController(FanaticServeContext context)
               {
        _context = context;
    }

    [HttpGet]
    // GET: RoleOnTrackController/Edit/5
    public ActionResult Edit(Guid? TrackId)
    {
        // idがnullの場合はBadRequestを返す
        if (TrackId == null)
        {
            return BadRequest();
        }

        // ViewDataにLabelマスタをセット
        ViewData["RoleList"] = new RoleService(_context).GetRoles();

        var EditRoleOnTrack = new EditRoleOnTrack();

        return View(EditRoleOnTrack);
    }

}
