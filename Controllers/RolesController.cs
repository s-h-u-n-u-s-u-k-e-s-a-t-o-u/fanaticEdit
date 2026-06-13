using fanaticEdit.Data;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class RolesController : Controller
{
    private readonly FanaticServeContext _context;

    public RolesController(FanaticServeContext context)
    {
        _context = context;
    }

    // GET: Roles
    public async Task<IActionResult> Index()
    {
        return View(await _context.Roles.ToListAsync());
    }

    // GET: Roles/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _context.Roles
            .FirstOrDefaultAsync(m => m.RoleId == id);
        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    // GET: Roles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Roles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoleId,Name,CreatedAt,ModifiedAt")] Role role)
    {
        if (ModelState.IsValid)
        {
            _context.Add(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(role);
    }

    // GET: Roles/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    // POST: Roles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("RoleId,Name,CreatedAt,ModifiedAt")] Role role)
    {
        if (id != role.RoleId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(role.RoleId))
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
        return View(role);
    }

    // GET: Roles/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _context.Roles
            .FirstOrDefaultAsync(m => m.RoleId == id);
        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    // POST: Roles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoleExists(int id)
    {
        return _context.Roles.Any(e => e.RoleId == id);
    }
}
