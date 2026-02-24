using MES_Lite.Data;
using MES_Lite.MesEntities;
using MES_Lite.Web.Common;
using MES_Lite.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MES_Lite.Web.Controllers
{
    public class MaterialLotsController : Controller
    {
        private readonly MesLiteDbContext _context;
        private readonly IConfiguration Configuration;

        public MaterialLotsController(MesLiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: MaterialLots
        public async Task<IActionResult> Index(string searchid,int searchmatdefid,int searchqty,
                                               string searchstatus,string searchsupplier,
                                               string searchcreatedat,string searchexpire,int? pageIndex)
        {
            MaterialLotViewModel materialLotViewModel = new()
            {
                CurrentFilterId = searchid,
                CurrentFilterMatdefId = searchmatdefid,
                CurrentFilterQuantity = searchqty,
                CurrentFilterStatus = searchstatus,
                CurrentFilterSupplier = searchsupplier,
                CurrentFilterCreatedAt = searchcreatedat,
                CurrentFilterExpiration = searchexpire

            };

            IQueryable<MaterialLot> query = _context.MaterialLots.Include(m => m.MaterialDefinition);

            //Filtri presenti nella query
            if (!string.IsNullOrEmpty(searchid))
            {
                query = query.Where(l => l.LotId.Contains(searchid));
            }
            if (searchmatdefid > 0)
            {
                query = query.Where(l => l.MaterialDefinitionId == searchmatdefid);
            }
            if (searchqty > 0)
            {
                query = query.Where(l => l.Quantity == searchmatdefid);
            }
            if (!string.IsNullOrEmpty(searchstatus))
            {
                query = query.Where(l => l.Status.Contains(searchstatus));
            }
            if (!string.IsNullOrEmpty(searchsupplier))
            {
                query = query.Where(l => l.Supplier.Contains(searchsupplier));
            }
            if (!string.IsNullOrEmpty(searchcreatedat) && DateOnly.TryParse(searchcreatedat, out var parsedDateAt))
            {
                query = query.Where(l => DateOnly.FromDateTime(l.CreatedAt) == parsedDateAt);
            }
            if (!string.IsNullOrEmpty(searchexpire) && DateOnly.TryParse(searchexpire, out var parsedDateEx))
            {
                query = query.Where(l => DateOnly.FromDateTime( l.CreatedAt) == parsedDateEx);
            }

            var pageSize = Configuration.GetValue("PageSize", 10);

            materialLotViewModel.MaterialLotsList = await PaginatedList<MaterialLot>.CreateAsync(
                query.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(materialLotViewModel);
        }

        // GET: MaterialLots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialLot = await _context.MaterialLots
                .Include(m => m.MaterialDefinition)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialLot == null)
            {
                return NotFound();
            }

            return View(materialLot);
        }

        // GET: MaterialLots/Create
        public IActionResult Create()
        {
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description");
            return View();
        }

        // POST: MaterialLots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LotId,MaterialDefinitionId,Quantity,Status,Supplier,CreatedAt,ExpirationDate")] MaterialLot materialLot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materialLot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description", materialLot.MaterialDefinitionId);
            return View(materialLot);
        }

        // GET: MaterialLots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialLot = await _context.MaterialLots.FindAsync(id);
            if (materialLot == null)
            {
                return NotFound();
            }
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description", materialLot.MaterialDefinitionId);
            return View(materialLot);
        }

        // POST: MaterialLots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LotId,MaterialDefinitionId,Quantity,Status,Supplier,CreatedAt,ExpirationDate")] MaterialLot materialLot)
        {
            if (id != materialLot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialLot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialLotExists(materialLot.Id))
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
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description", materialLot.MaterialDefinitionId);
            return View(materialLot);
        }

        // GET: MaterialLots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialLot = await _context.MaterialLots
                .Include(m => m.MaterialDefinition)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialLot == null)
            {
                return NotFound();
            }

            return View(materialLot);
        }

        // POST: MaterialLots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialLot = await _context.MaterialLots.FindAsync(id);
            if (materialLot != null)
            {
                _context.MaterialLots.Remove(materialLot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialLotExists(int id)
        {
            return _context.MaterialLots.Any(e => e.Id == id);
        }
    }
}
