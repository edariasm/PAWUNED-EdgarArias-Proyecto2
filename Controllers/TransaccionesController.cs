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
    public class TransaccionesController : Controller
    {
        private readonly Proyecto2Context _context;

        public TransaccionesController(Proyecto2Context context)
        {
            _context = context;
        }

        // GET: Transacciones
        public async Task<IActionResult> Index()
        {
            var proyecto2Context = _context.Transacciones.Include(t => t.IdObraNavigation).Include(t => t.IdUsuarioCompradorNavigation);
            return View(await proyecto2Context.ToListAsync());
        }

        // GET: Transacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones
                .Include(t => t.IdObraNavigation)
                .Include(t => t.IdUsuarioCompradorNavigation)
                .FirstOrDefaultAsync(m => m.IdTransaccion == id);
            if (transaccione == null)
            {
                return NotFound();
            }

            return View(transaccione);
        }

        // GET: Transacciones/Create
        public IActionResult Create()
        {
            ViewData["IdObra"] = new SelectList(_context.ObrasArtes, "IdObra", "IdObra");
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Transacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTransaccion,IdUsuarioComprador,IdUsuarioArtista,IdObra,MontoTransaccion,FechaTransaccion")] Transaccione transaccione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaccione);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Galeria");
            }
            ViewData["IdObra"] = new SelectList(_context.ObrasArtes, "IdObra", "IdObra", transaccione.IdObra);
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", transaccione.IdUsuarioComprador);
            return View(transaccione);
        }

        // GET: Transacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones.FindAsync(id);
            if (transaccione == null)
            {
                return NotFound();
            }
            ViewData["IdObra"] = new SelectList(_context.ObrasArtes, "IdObra", "IdObra", transaccione.IdObra);
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", transaccione.IdUsuarioComprador);
            return View(transaccione);
        }

        // POST: Transacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTransaccion,IdUsuarioComprador,IdUsuarioArtista,IdObra,MontoTransaccion,FechaTransaccion")] Transaccione transaccione)
        {
            if (id != transaccione.IdTransaccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccioneExists(transaccione.IdTransaccion))
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
            ViewData["IdObra"] = new SelectList(_context.ObrasArtes, "IdObra", "IdObra", transaccione.IdObra);
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", transaccione.IdUsuarioComprador);
            return View(transaccione);
        }

        // GET: Transacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones
                .Include(t => t.IdObraNavigation)
                .Include(t => t.IdUsuarioCompradorNavigation)
                .FirstOrDefaultAsync(m => m.IdTransaccion == id);
            if (transaccione == null)
            {
                return NotFound();
            }

            return View(transaccione);
        }

        // POST: Transacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccione = await _context.Transacciones.FindAsync(id);
            if (transaccione != null)
            {
                _context.Transacciones.Remove(transaccione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccioneExists(int id)
        {
            return _context.Transacciones.Any(e => e.IdTransaccion == id);
        }
    }
}
