using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWUNED_EdgarArias_Proyecto2.Models;

namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{
    public class SubastasController : Controller
    {
        private readonly Proyecto2Context _context;

        public SubastasController(Proyecto2Context context)
        {
            _context = context;
        }

        // GET: Subastas
        public async Task<IActionResult> Index()
        {
            var proyecto2Context = _context.Subastas.Include(s => s.IdUsuarioGanadorNavigation);
            return View(await proyecto2Context.ToListAsync());
        }

        // GET: Subastas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subasta = await _context.Subastas
                .Include(s => s.IdUsuarioGanadorNavigation)
                .FirstOrDefaultAsync(m => m.IdSubasta == id);
            if (subasta == null)
            {
                return NotFound();
            }

            return View(subasta);
        }

        // GET: Subastas/Create
        public IActionResult Create()
        {
            ViewData["IdUsuarioGanador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Subastas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSubasta,IdObra,PrecioActual,FechaHoraCierre,IdUsuarioGanador")] Subasta subasta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subasta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Galeria");
            }
            ViewData["IdUsuarioGanador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", subasta.IdUsuarioGanador);
            return View(subasta);
        }

        // GET: Subastas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subasta = await _context.Subastas.FindAsync(id);
            if (subasta == null)
            {
                return NotFound();
            }
            ViewData["IdUsuarioGanador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", subasta.IdUsuarioGanador);
            return View(subasta);
        }

        // POST: Subastas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSubasta,IdObra,PrecioActual,FechaHoraCierre,IdUsuarioGanador")] Subasta subasta)
        {
            if (id != subasta.IdSubasta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subasta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubastaExists(subasta.IdSubasta))
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
            ViewData["IdUsuarioGanador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", subasta.IdUsuarioGanador);
            return View(subasta);
        }

        // GET: Subastas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subasta = await _context.Subastas
                .Include(s => s.IdUsuarioGanadorNavigation)
                .FirstOrDefaultAsync(m => m.IdSubasta == id);
            if (subasta == null)
            {
                return NotFound();
            }

            return View(subasta);
        }

        // POST: Subastas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subasta = await _context.Subastas.FindAsync(id);
            if (subasta != null)
            {
                _context.Subastas.Remove(subasta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubastaExists(int id)
        {
            return _context.Subastas.Any(e => e.IdSubasta == id);
        }

        // GET: Subastas ganadas por el usuario actual
        public async Task<IActionResult> MisSubastasGanadas()
        {
            // Obtener el ID del usuario actual desde la sesión
            var userId = HttpContext.Session.GetString("id");

            // Convertir el ID de usuario a entero
            var userIdInt = int.Parse(userId);

            // Obtener las subastas ganadas por el usuario actual
            var subastasGanadas = await _context.Subastas
                .Where(s => s.IdUsuarioGanador == userIdInt)
                   .Include(s => s.IdUsuarioGanadorNavigation)
                .ToListAsync();

            return View(subastasGanadas);
        }


    }
}
