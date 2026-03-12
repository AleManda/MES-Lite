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


        //_______________________________________________________________________________________
        //_______________________________________________________________________________________
        //CRUD standard per MaterialDefinition, con filtri di ricerca e paginazione nella Index
        //_______________________________________________________________________________________
        //_______________________________________________________________________________________


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

            // MaterialId è un identificatore di dominio, quindi uso Contains per permettere ricerche parziali
            if (!string.IsNullOrEmpty(searchid))  
            {
                query = query.Where(m => m.MaterialId.Contains(searchid));
            }

            // Description è un campo testuale, quindi uso Contains per permettere ricerche parziali
            if (!string.IsNullOrEmpty(searchdescr)) 
            {
                query = query.Where(m => m.Description.Contains(searchdescr));
            }

            // Version è un campo testuale, quindi uso Contains per permettere ricerche parziali
            if (!string.IsNullOrEmpty(searchversion)) 
            {
                query = query.Where(m => m.Version.Contains(searchversion));
            }

            // UoM è un campo testuale, quindi uso Contains per permettere ricerche parziali
            if (!string.IsNullOrEmpty(searchuom))  
            {
                query = query.Where(m => m.UoM.Contains(searchuom));
            }

            // MaterialClassId è un identificatore di dominio, quindi uso l'uguaglianza
            if (searchclassid > 0)
            {
                query = query.Where(m => m.MaterialClassId == searchclassid);
            }

            // Specification è un campo testuale, quindi uso Contains per permettere ricerche parziali
            if (!string.IsNullOrEmpty(searchspec)) 
            {
                query = query.Where(m => m.Specification.Contains(searchspec));
            }

            // Supplier è un campo testuale, quindi uso Contains per permettere ricerche parziali
            if (!string.IsNullOrEmpty(searchsupplier)) 
            {
                query = query.Where(m => m.Supplier.Contains(searchsupplier));
            }

            // La proiezione viene fatta a livello di database, quindi solo i campi necessari vengono recuperati
            IQueryable<MaterialDefinitionModel> query2 = query.Select(p => new MaterialDefinitionModel
            {
                Id = p.Id,
                MaterialId = p.MaterialId,
                MaterialClassId = p.MaterialClassId,
                Description = p.Description,
                Version = p.Version,
                Conformity = p.Conformity,
                Specification = p.Specification,
                Supplier = p.Supplier,
                UoM = p.UoM,
                Critical = p.Critical,
                RequiresDoubleCheck = p.RequiresDoubleCheck,
            });

            var pageSize = Configuration.GetValue("PageSize", 10);

            // La paginazione viene applicata a livello di database(skip/take), quindi solo i record
            // necessari per la pagina corrente vengono recuperati
            materialDefinitionViewModel.MaterialDefsList = await PaginatedList<MaterialDefinitionModel>.CreateAsync(
                query2.AsNoTracking(), pageIndex ?? 1, pageSize);

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

            //var materialDefinition = await _context.MaterialDefinitions.FirstOrDefaultAsync(m => m.Id == id);
            var materialDefinition = await _context.MaterialDefinitions.FindAsync(id);
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

            try
            {
                if (materialDefinition != null)
                {
                    _context.MaterialDefinitions.Remove(materialDefinition);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] =
                    "Impossibile eliminare questo materiale.Controllare se ci sono Material Requirements o lotti associati";

                return View();
            }
        }

        private bool MaterialDefinitionExists(int id)
        {
            return _context.MaterialDefinitions.Any(e => e.Id == id);
        }
    }
}
