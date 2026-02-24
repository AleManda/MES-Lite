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
    public class PersonnelsController : Controller
    {
        private readonly MesLiteDbContext _context;
        private readonly IConfiguration Configuration;

        public PersonnelsController(MesLiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: Personnels
        public async Task<IActionResult> Index(string searchid,string searchname,string searchrole,
                                               string searchqual,int? pageIndex)
        {
            PersonnelViewModel personnelViewModel = new()
            {
                CurrentFilterPersonId = searchid,
                CurrentFilterName = searchname,
                CurrentFilterRole = searchrole,
                CurrentFilterQualification = searchqual
            };

            IQueryable<Personnel> query = _context.Personnel;

            if (!string.IsNullOrEmpty(searchid))
            {
                query = query.Where(p => p.PersonId.Contains(searchid));
            }
            if (!string.IsNullOrEmpty(searchname))
            {
                query = query.Where(p => p.Name.Contains(searchname));
            }
            if (!string.IsNullOrEmpty(searchrole))
            {
                query = query.Where(p => p.Role.Contains(searchrole));
            }
            if (!string.IsNullOrEmpty(searchqual))
            {
                query = query.Where(p => p.Qualification.Contains(searchqual));
            }

            var pageSize = Configuration.GetValue("PageSize", 10);

            personnelViewModel.PersonnelList = await PaginatedList<Personnel>.CreateAsync(
                query.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(personnelViewModel);

            //return View(await _context.Personnel.ToListAsync());
        }

        // GET: Personnels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personnel == null)
            {
                return NotFound();
            }

            return View(personnel);
        }

        // GET: Personnels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personnels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PersonId,Name,Role,Qualification")] Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personnel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personnel);
        }

        // GET: Personnels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel.FindAsync(id);
            if (personnel == null)
            {
                return NotFound();
            }
            return View(personnel);
        }

        // POST: Personnels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PersonId,Name,Role,Qualification")] Personnel personnel)
        {
            if (id != personnel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personnel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonnelExists(personnel.Id))
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
            return View(personnel);
        }

        // GET: Personnels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personnel == null)
            {
                return NotFound();
            }

            return View(personnel);
        }

        // POST: Personnels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personnel = await _context.Personnel.FindAsync(id);
            if (personnel != null)
            {
                _context.Personnel.Remove(personnel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonnelExists(int id)
        {
            return _context.Personnel.Any(e => e.Id == id);
        }
    }
}
