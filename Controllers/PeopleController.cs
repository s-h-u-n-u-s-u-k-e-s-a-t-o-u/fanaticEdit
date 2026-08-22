using fanaticEdit.Data;
using fanaticEdit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Controllers;

public class PeopleController : Controller
{
    private readonly FanaticServeContext _context;

    public PeopleController(FanaticServeContext context)
    {
        _context = context;
    }

    // GET: People
    public async Task<IActionResult> Index()
    {
        return View(await _context.People.ToListAsync());
    }

    // GET: People/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var person = await _context.People
            .FirstOrDefaultAsync(m => m.PersonId == id);
        if (person == null)
        {
            return NotFound();
        }

        return View(person);
    }

    // GET: People/Create
    public IActionResult Create()
    {
        return View(
            new Person()
            {
                PersonId = Guid.NewGuid(),

                // JCTで作成日時と更新日時を設定する
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }
            );
    }

    // POST: People/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PersonId,Name,Kana,CreatedAt,ModifiedAt")] Person person)
    {
        if (ModelState.IsValid)
        {
            person.PersonId = Guid.NewGuid();
            _context.Add(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(person);
    }

    // GET: People/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    // POST: People/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("PersonId,Name,Kana,CreatedAt,ModifiedAt")] Person person)
    {
        if (id != person.PersonId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            person.ModifiedAt = DateTime.Now;

            try
            {
                _context.Update(person);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(person.PersonId))
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
        return View(person);
    }

    // GET: People/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var person = await _context.People
            .FirstOrDefaultAsync(m => m.PersonId == id);
        if (person == null)
        {
            return NotFound();
        }

        return View(person);
    }

    // POST: People/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var person = await _context.People.FindAsync(id);
        if (person != null)
        {
            _context.People.Remove(person);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PersonExists(Guid id)
    {
        return _context.People.Any(e => e.PersonId == id);
    }
}
