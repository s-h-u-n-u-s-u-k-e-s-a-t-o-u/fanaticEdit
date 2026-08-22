using fanaticEdit.Data;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class LabelsController : Controller
{
    private readonly FanaticServeContext _context;
    private readonly ILogger<LabelsController> _logger; // ILoggerを追加

    public LabelsController(FanaticServeContext context, ILogger<LabelsController> logger)
    {
        _context = context;
        _logger = logger; // ILoggerのインスタンスを取得
    }

    // GET: Labels
    public async Task<IActionResult> Index()
    {
        return View(await _context.Labels.ToListAsync());
    }

    // GET: Labels/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var label = await _context.Labels
            .FirstOrDefaultAsync(m => m.LabelId == id);
        if (label == null)
        {
            return NotFound();
        }

        return View(label);
    }

    // GET: Labels/Create
    public IActionResult Create()
    {
        ViewData["OrganizationList"] = GetOrganizations();


        var model = new Models.Label()
        {
            LabelId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
        return View(model);
    }

    // POST: Labels/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LabelId,OrganizationId,Name,CreatedAt,ModifiedAt")] Label label)
    {
        if (ModelState.IsValid)
        {
            label.LabelId = Guid.NewGuid();
            _context.Add(label);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(label);
    }

    // GET: Labels/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        ViewData["OrganizationList"] = GetOrganizations();
        if (id == null)
        {
            return NotFound();
        }

        var label = await _context.Labels.FindAsync(id);
        if (label == null)
        {
            return NotFound();
        }
        return View(label);
    }

    // POST: Labels/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("LabelId,OrganizationId,Name,CreatedAt,ModifiedAt")] Label label)
    {
        if (id != label.LabelId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(label);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelExists(label.LabelId))
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
                _logger.LogError(ex, "Label処理エラー");
                
                ModelState.AddModelError("", "Labelの処理に失敗しました");
            }
            return RedirectToAction(nameof(Index));
        }
        return View(label);
    }

    // GET: Labels/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var label = await _context.Labels
            .FirstOrDefaultAsync(m => m.LabelId == id);
        if (label == null)
        {
            return NotFound();
        }

        return View(label);
    }

    // POST: Labels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var label = await _context.Labels.FindAsync(id);
        if (label != null)
        {
            _context.Labels.Remove(label);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LabelExists(Guid id)
    {
        return _context.Labels.Any(e => e.LabelId == id);
    }

    // OrganizationのListを取得する
    private SelectList GetOrganizations()
    {
        var items = _context.Organizations
            .Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.OrganizationId.ToString()
            })
            .ToList();

        return new SelectList(items, "Value", "Text");
    }
}
