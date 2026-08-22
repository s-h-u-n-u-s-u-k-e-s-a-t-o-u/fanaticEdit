using fanaticEdit.Data;
using fanaticEdit.DTO;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class AbstractEventsController : Controller
{
    private readonly FanaticServeContext _context;
    private readonly ILogger<AbstractEventsController> _logger;

    public AbstractEventsController(FanaticServeContext context, ILogger<AbstractEventsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: AbstractEvents
    public async Task<IActionResult> Index()
    {
        return View(await _context.AbstractEvents.OrderBy(m => m.Title).ToListAsync());
    }

    // GET: AbstractEvents/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var abstractEvent = await _context.AbstractEvents
            .FirstOrDefaultAsync(m => m.AbstractEventId == id);
        if (abstractEvent == null)
        {
            return NotFound();
        }

        return View(abstractEvent);
    }

    // GET: AbstractEvents/Create
    public IActionResult Create()
    {
        return View(
            new AbstractEvent
            {
                AbstractEventId = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }
            );
    }

    // POST: AbstractEvents/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AbstractEventId,Title,CreatedAt,ModifiedAt")] AbstractEvent abstractEvent)
    {
        if (ModelState.IsValid)
        {
            abstractEvent.AbstractEventId = Guid.NewGuid();
            _context.Add(abstractEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(abstractEvent);
    }

    // GET: AbstractEvents/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var abstractEvent = await _context.AbstractEvents.FindAsync(id);
        if (abstractEvent == null)
        {
            return NotFound();
        }

        abstractEvent.LinkedEvents = await (
            from abstEventLink in _context.AbstractEventLinks
            join liveEvent in _context.LiveEvents on abstEventLink.EventId equals liveEvent.LiveEventId
            where abstEventLink.AbstractEventId == id
            select new LinkedEvent()
            {
                Link = abstEventLink,
                LiveEvent = liveEvent
            }
            ).ToListAsync();

        return View(abstractEvent);
    }

    // POST: AbstractEvents/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("AbstractEventId,Title,CreatedAt,ModifiedAt")] AbstractEvent abstractEvent)
    {
        if (id != abstractEvent.AbstractEventId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(abstractEvent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbstractEventExists(abstractEvent.AbstractEventId))
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
                _logger.LogError(ex, "AbstractEvent 処理エラー");
                // または
                ModelState.AddModelError("", "AbstractEvent の処理に失敗しました");
            }
            return RedirectToAction(nameof(Index));
        }
        return View(abstractEvent);
    }

    // GET: AbstractEvents/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var abstractEvent = await _context.AbstractEvents
            .FirstOrDefaultAsync(m => m.AbstractEventId == id);
        if (abstractEvent == null)
        {
            return NotFound();
        }

        return View(abstractEvent);
    }

    // POST: AbstractEvents/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var abstractEvent = await _context.AbstractEvents.FindAsync(id);
        if (abstractEvent != null)
        {
            _context.AbstractEvents.Remove(abstractEvent);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AbstractEventExists(Guid id)
    {
        return _context.AbstractEvents.Any(e => e.AbstractEventId == id);
    }
}
