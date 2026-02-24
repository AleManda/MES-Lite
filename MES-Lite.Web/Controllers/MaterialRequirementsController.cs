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
    public class MaterialRequirementsController : Controller
    {
        private readonly MesLiteDbContext _context;
        private readonly IConfiguration Configuration;

        public MaterialRequirementsController(MesLiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: MaterialRequirements
        public async Task<IActionResult> Index(int searckworkorderid,int searchmatdefid,decimal searchqty,int? pageIndex)
        {
            MaterialRequirementViewModel materialRequirementViewModel = new()
            {
                CurrentFilterWorkOrderId = searckworkorderid,
                CurrentFilterMatDefId = searchmatdefid,
                CurrentFilterQuantity = searchqty
            };

            IQueryable<MaterialRequirement> query = _context.MaterialRequirements.Include(m => m.MaterialDefinition).Include(m => m.WorkOrder);

            //filtri
            if(searckworkorderid > 0)
            {
                query = query.Where(m => m.WorkOrderId == searckworkorderid);
            }
            if (searchmatdefid > 0)
            {
                query = query.Where(m => m.MaterialDefinitionId == searckworkorderid);
            }
            if (searchqty > 0)
            {
                query = query.Where(m => m.RequiredQuantity == searchqty);
            }

            var pageSize = Configuration.GetValue("PageSize", 10);

            materialRequirementViewModel.MaterialRequirementList = await PaginatedList<MaterialRequirement>.CreateAsync(
                query.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(materialRequirementViewModel);

            //var mesLiteDbContext = _context.MaterialRequirements.Include(m => m.MaterialDefinition).Include(m => m.WorkOrder);
            //return View(await mesLiteDbContext.ToListAsync());
        }

        // GET: MaterialRequirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialRequirement = await _context.MaterialRequirements
                .Include(m => m.MaterialDefinition)
                .Include(m => m.WorkOrder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialRequirement == null)
            {
                return NotFound();
            }

            return View(materialRequirement);
        }

        // GET: MaterialRequirements/Create
        public IActionResult Create()
        {
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description");
            ViewData["WorkOrderId"] = new SelectList(_context.WorkOrders, "Id", "Description");
            return View();
        }

        // POST: MaterialRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkOrderId,MaterialDefinitionId,RequiredQuantity")] MaterialRequirement materialRequirement)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(materialRequirement);
                await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description", materialRequirement.MaterialDefinitionId);
            ViewData["WorkOrderId"] = new SelectList(_context.WorkOrders, "Id", "Description", materialRequirement.WorkOrderId);
            return View(materialRequirement);
        }

        // GET: MaterialRequirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialRequirement = await _context.MaterialRequirements.FindAsync(id);
            if (materialRequirement == null)
            {
                return NotFound();
            }
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description", materialRequirement.MaterialDefinitionId);
            ViewData["WorkOrderId"] = new SelectList(_context.WorkOrders, "Id", "Description", materialRequirement.WorkOrderId);
            return View(materialRequirement);
        }

        // POST: MaterialRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkOrderId,MaterialDefinitionId,RequiredQuantity")] MaterialRequirement materialRequirement)
        {
            if (id != materialRequirement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialRequirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialRequirementExists(materialRequirement.Id))
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
            ViewData["MaterialDefinitionId"] = new SelectList(_context.MaterialDefinitions, "Id", "Description", materialRequirement.MaterialDefinitionId);
            ViewData["WorkOrderId"] = new SelectList(_context.WorkOrders, "Id", "Description", materialRequirement.WorkOrderId);
            return View(materialRequirement);
        }

        // GET: MaterialRequirements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialRequirement = await _context.MaterialRequirements
                .Include(m => m.MaterialDefinition)
                .Include(m => m.WorkOrder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialRequirement == null)
            {
                return NotFound();
            }

            return View(materialRequirement);
        }

        // POST: MaterialRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialRequirement = await _context.MaterialRequirements.FindAsync(id);
            if (materialRequirement != null)
            {
                _context.MaterialRequirements.Remove(materialRequirement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialRequirementExists(int id)
        {
            return _context.MaterialRequirements.Any(e => e.Id == id);
        }
    }
}
