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
    public class WorkOrdersController : Controller
    {
        private readonly MesLiteDbContext _context;
        private readonly IConfiguration Configuration;

        public WorkOrdersController(MesLiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: WorkOrders
        public async Task<IActionResult> Index(string searchworderid,string searchdescr,string searchstartdate,
                                               string searchenddate,string searchstatus,int? pageIndex)
        {
            WorkOrderViewModel workOrderViewModel = new()
            {
                CurrentFilterWorkOrderId = searchworderid,
                CurrentFilterDescr = searchdescr,
                CurrentFilterSchedStart = searchstartdate,
                CurrentFilterSchedEnd = searchenddate,
                CurrentFilterStatus = searchstatus
            };

            IQueryable<WorkOrder> query = _context.WorkOrders;

            if (!string.IsNullOrEmpty(searchworderid))
            {
                query = query.Where(w => w.WorkOrderId.Contains(searchworderid));
            }
            if (!string.IsNullOrEmpty(searchdescr))
            {
                query = query.Where(w => w.Description.Contains(searchdescr));
            }
            if (!string.IsNullOrEmpty(searchstartdate) && DateOnly.TryParse(searchstartdate, out var parsedDatestart))
            {
                query = query.Where(w => DateOnly.FromDateTime(w.ScheduledStart) == parsedDatestart);
            }
            if (!string.IsNullOrEmpty(searchenddate) && DateOnly.TryParse(searchenddate, out var parsedDateend))
            {
                query = query.Where(w => DateOnly.FromDateTime(w.ScheduledEnd) == parsedDateend);
            }
            if (!string.IsNullOrEmpty(searchstatus))
            {
                query = query.Where(w => w.Status.Contains(searchstatus));
            }

            var pageSize = Configuration.GetValue("PageSize", 10);

            workOrderViewModel.WorkOrderList = await PaginatedList<WorkOrder>.CreateAsync(
                query.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(workOrderViewModel);

            //return View(await _context.WorkOrders.ToListAsync());
        }

        // GET: WorkOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // GET: WorkOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkOrderId,Description,ScheduledStart,ScheduledEnd,Status")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workOrder);
        }

        // GET: WorkOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return NotFound();
            }
            return View(workOrder);
        }

        // POST: WorkOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkOrderId,Description,ScheduledStart,ScheduledEnd,Status")] WorkOrder workOrder)
        {
            if (id != workOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkOrderExists(workOrder.Id))
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
            return View(workOrder);
        }

        // GET: WorkOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workOrder = await _context.WorkOrders.FindAsync(id);
            if (workOrder != null)
            {
                _context.WorkOrders.Remove(workOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkOrderExists(int id)
        {
            return _context.WorkOrders.Any(e => e.Id == id);
        }
    }
}
