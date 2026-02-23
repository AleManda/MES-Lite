using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MES_Lite.Data;
using MES_Lite.MesEntities;

namespace MES_Lite.Web.Controllers
{
    public class MaterialDefinitionsController : Controller
    {
        private readonly MesLiteDbContext _context;

        public MaterialDefinitionsController(MesLiteDbContext context)
        {
            _context = context;
        }

        // GET: MaterialDefinitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaterialDefinitions.ToListAsync());
        }

        // GET: MaterialDefinitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialDefinition = await _context.MaterialDefinitions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialDefinition == null)
            {
                return NotFound();
            }

            return View(materialDefinition);
        }

        // GET: MaterialDefinitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MaterialDefinitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaterialId,Description,Version,UoM,MaterialClassId,Specification,Supplier,Conformity,Critical,RequiresDoubleCheck")] MaterialDefinition materialDefinition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materialDefinition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materialDefinition);
        }

        // GET: MaterialDefinitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialDefinition = await _context.MaterialDefinitions.FindAsync(id);
            if (materialDefinition == null)
            {
                return NotFound();
            }
            return View(materialDefinition);
        }

        // POST: MaterialDefinitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaterialId,Description,Version,UoM,MaterialClassId,Specification,Supplier,Conformity,Critical,RequiresDoubleCheck")] MaterialDefinition materialDefinition)
        {
            if (id != materialDefinition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialDefinition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialDefinitionExists(materialDefinition.Id))
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
            return View(materialDefinition);
        }

        // GET: MaterialDefinitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialDefinition = await _context.MaterialDefinitions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialDefinition == null)
            {
                return NotFound();
            }

            return View(materialDefinition);
        }

        // POST: MaterialDefinitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialDefinition = await _context.MaterialDefinitions.FindAsync(id);
            if (materialDefinition != null)
            {
                _context.MaterialDefinitions.Remove(materialDefinition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialDefinitionExists(int id)
        {
            return _context.MaterialDefinitions.Any(e => e.Id == id);
        }
    }
}
