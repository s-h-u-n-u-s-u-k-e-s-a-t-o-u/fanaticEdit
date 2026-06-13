using Microsoft.AspNetCore.Mvc.Rendering;

namespace fanaticEdit.Data;

public class RoleService
{
    private readonly FanaticServeContext _context;

    public RoleService(FanaticServeContext context)
    {
        _context = context;
    }

    // LabelのListを取得する
    public SelectList GetRoles()
    {
        var items =
             _context.Roles.OrderBy(r => r.Name)
            .Select(l => new SelectListItem
            {
                Text = l.Name,
                Value = l.RoleId.ToString()
            }
            ).ToList()
            ;

        return new SelectList(items, "Value", "Text");
    }
}
