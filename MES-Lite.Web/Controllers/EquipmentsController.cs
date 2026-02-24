using MES_Lite.Data;
using MES_Lite.MesEntities;
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
    public class EquipmentsController : Controller
    {
        private readonly MesLiteDbContext _context;
        private readonly IConfiguration Configuration;

        public EquipmentsController(MesLiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(string searchid,string searchdescr,string searchclassid,string searchlocation,
                                               string searchstatus,int? pageIndex)
        {
            EquipmentViewModel equipmentViewModel = new()
            {
                CurrentFilterId = searchid,
                CurrentFilterDescr = searchdescr,
                CurrentFilterClassId = searchclassid,
                CurrentFilterLocation = searchlocation,
                CurrentFilterStatus = searchstatus

            };

            IQueryable<Equipment> query = _context.Equipment;


            if (!string.IsNullOrEmpty(searchid))
            {
                query = query.Where(e => e.EquipmentId.Contains(searchid));
            }
            if (!string.IsNullOrEmpty(searchdescr))
            {
                query = query.Where(e => e.Description.Contains(searchdescr));
            }
            if (!string.IsNullOrEmpty(searchclassid))
            {
                query = query.Where(e => e.EquipmentClassId.Contains(searchclassid));
            }
            if (!string.IsNullOrEmpty(searchlocation))
            {
                query = query.Where(e => e.Location.Contains(searchlocation));
            }
            if (!string.IsNullOrEmpty(searchstatus))
            {
                query = query.Where(e => e.Status.Contains(searchstatus));
            }

            var pageSize = Configuration.GetValue("PageSize", 10);

            equipmentViewModel.EquipmentList = await PaginatedList<Equipment>.CreateAsync(
                query.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(equipmentViewModel);
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EquipmentId,Description,EquipmentClassId,Location,Status")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EquipmentId,Description,EquipmentClassId,Location,Status")] Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
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
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Id == id);
        }
    }
}
