using MES_Lite.Data;
using MES_Lite.MesEntities;
using MES_Lite.Web.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MES_Lite.Web.Models;

namespace MES_Lite.Web.Controllers
{
    public class MaterialDefinitionsController : Controller
    {
        private readonly MesLiteDbContext _context;
        private readonly IConfiguration Configuration;




        public MaterialDefinitionsController(MesLiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        //_________________________________________________________________________________________
        // GET: MaterialDefinitions
        public async Task<IActionResult> Index(string searchid,string searchdescr,string searchversion,string searchuom,
            int searchclassid,string searchspec,string searchsupplier,int? pageIndex)
        {

            MaterialDefinitionViewModel materialDefinitionViewModel = new()
            {
                CurrentFilterId = searchid,
                CurrentFilterDescr = searchdescr,
                CurrentFilterVersion = searchversion,
                CurrentFilterMatClassId = searchclassid,
                CurrentFilterSpec = searchspec,
                CurrentFilterSupplier = searchsupplier,
                CurrentFilterUoM = searchuom

            };

            //query di base
            IQueryable<MaterialDefinition> query = _context.MaterialDefinitions;

            //Filtri presenti nella query
            if (!string.IsNullOrEmpty(searchid))
            {
                query = query.Where(m => m.MaterialId.Contains(searchid));
            }
            if (!string.IsNullOrEmpty(searchdescr))
            {
                query = query.Where(m => m.Description.Contains(searchdescr));
            }
            if (!string.IsNullOrEmpty(searchversion))
            {
                query = query.Where(m => m.Version.Contains(searchversion));
            }

            if (searchclassid > 0)
                query = query.Where(m => m.MaterialClassId == searchclassid);

            if (!string.IsNullOrEmpty(searchspec))
            {
                query = query.Where(m => m.Specification.Contains(searchspec));
            }
            if (!string.IsNullOrEmpty(searchsupplier))
            {
                query = query.Where(m => m.Supplier.Contains(searchsupplier));
            }


            var pageSize = Configuration.GetValue("PageSize", 10);

            // 
            materialDefinitionViewModel.MaterialDefsList = await PaginatedList<MaterialDefinition>.CreateAsync(
                query.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(materialDefinitionViewModel);
        }

        //_________________________________________________________________________________________
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

        //_________________________________________________________________________________________
        // GET: MaterialDefinitions/Create
        public IActionResult Create()
        {
            return View();
        }

        //_________________________________________________________________________________________
        // POST: MaterialDefinitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,MaterialId,Description,Version,UoM,MaterialClassId,Specification,Supplier,Conformity,Critical,RequiresDoubleCheck")] 
                          MaterialDefinition materialDefinition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materialDefinition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materialDefinition);
        }

        //_________________________________________________________________________________________
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

        //_________________________________________________________________________________________
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

        //_________________________________________________________________________________________
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


        //_________________________________________________________________________________________
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
