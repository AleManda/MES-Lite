using MES_Lite.Data;
using MES_Lite.MesEntities;
using MES_Lite.MesEntities.Enums;
using MES_Lite.Web.Common;
using MES_Lite.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MES_Lite.Web.Services;

namespace MES_Lite.Web.Controllers
{
    public class WorkOrdersController : Controller
    {
        private readonly MesLiteDbContext _context;
        private readonly IConfiguration Configuration;
        private readonly IWorkOrderWorkflowService _workflow;

        public WorkOrdersController(MesLiteDbContext context, IConfiguration configuration, IWorkOrderWorkflowService wf)
        {
            _context = context;
            _workflow = wf;
            Configuration = configuration;
        }

        //__________________________________________________________________________________________
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
                //query = query.Where(w => w.Status.ToString().Contains(searchstatus));
                query = query.Where(w => EF.Property<string>(w, "Status").Contains(searchstatus));
            }

            IQueryable<WorkOrderModel> query2 = query.Select(p => new WorkOrderModel
            {
                Id = p.Id,
                WorkOrderId = p.WorkOrderId,
                Status = p.Status.ToString(),
                Description = p.Description,
                ScheduledEnd = p.ScheduledEnd,
                ScheduledStart = p.ScheduledStart
            });

            var pageSize = Configuration.GetValue("PageSize", 10);

            workOrderViewModel.WorkOrderList = await PaginatedList<WorkOrderModel>.CreateAsync(
                query2.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(workOrderViewModel);

            //return View(await _context.WorkOrders.ToListAsync());
        }

        //__________________________________________________________________________________________
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

        //__________________________________________________________________________________________
        // GET: WorkOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        //__________________________________________________________________________________________
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

        //__________________________________________________________________________________________
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

        //__________________________________________________________________________________________
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

        //__________________________________________________________________________________________
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

        //__________________________________________________________________________________________
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

        //__________________________________________________________________________________________
        private bool WorkOrderExists(int id)
        {
            return _context.WorkOrders.Any(e => e.Id == id);
        }


        //__________________________________________________________________________________________
        //__________________________________________________________________________________________
        //__________________________________________________________________________________________
        //__________________________________________________________________________________________
        //__________________________________________________________________________________________

        public async Task<IActionResult> ChangeStatus(int id, WorkOrderStatus newStatus)
        {
            var ok = await _workflow.TryChangeStatusAsync(id, newStatus);

            if (!ok)
                return BadRequest("Transizione di stato non consentita.");

            return RedirectToAction(nameof(Details), new { id });
        }

    }
}
