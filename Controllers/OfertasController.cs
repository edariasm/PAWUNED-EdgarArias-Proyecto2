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
    public class OfertasController : Controller
    {
        private readonly Proyecto2Context _context;

        public OfertasController(Proyecto2Context context)
        {
            _context = context;
        }

        // GET: Ofertas
        public async Task<IActionResult> Index()
        {
            var proyecto2Context = _context.Ofertas.Include(o => o.IdSubastaNavigation).Include(o => o.IdUsuarioCompradorNavigation);
            return View(await proyecto2Context.ToListAsync());
        }

        // GET: Ofertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Ofertas
                .Include(o => o.IdSubastaNavigation)
                .Include(o => o.IdUsuarioCompradorNavigation)
                .FirstOrDefaultAsync(m => m.IdOferta == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // GET: Ofertas/Create
        public IActionResult Create()
        {
            ViewData["IdSubasta"] = new SelectList(_context.Subastas, "IdSubasta", "IdSubasta");
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Ofertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOferta,IdSubasta,IdUsuarioComprador,MontoOferta")] Oferta oferta)
        {
            if (ModelState.IsValid)
            {

               //var idSubasta= ObtenerIdSubasta(idObra);

                

               //oferta.IdSubasta = idSubasta;

                _context.Add(oferta);
                await _context.SaveChangesAsync();
				// Pasar idSubasta a la vista utilizando ViewBag
				//ViewBag.IdSubasta = idSubasta;
				return RedirectToAction("Index", "Galeria");
				//return RedirectToAction(nameof(Index));
            }



            ViewData["IdSubasta"] = new SelectList(_context.Subastas, "IdSubasta", "IdSubasta", oferta.IdSubasta);
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", oferta.IdUsuarioComprador);
            return View(oferta);
        }

        // GET: Ofertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Ofertas.FindAsync(id);
            if (oferta == null)
            {
                return NotFound();
            }
            ViewData["IdSubasta"] = new SelectList(_context.Subastas, "IdSubasta", "IdSubasta", oferta.IdSubasta);
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", oferta.IdUsuarioComprador);
            return View(oferta);
        }

        // POST: Ofertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOferta,IdSubasta,IdUsuarioComprador,MontoOferta")] Oferta oferta)
        {
            if (id != oferta.IdOferta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oferta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfertaExists(oferta.IdOferta))
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
            ViewData["IdSubasta"] = new SelectList(_context.Subastas, "IdSubasta", "IdSubasta", oferta.IdSubasta);
            ViewData["IdUsuarioComprador"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", oferta.IdUsuarioComprador);
            return View(oferta);
        }

        // GET: Ofertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Ofertas
                .Include(o => o.IdSubastaNavigation)
                .Include(o => o.IdUsuarioCompradorNavigation)
                .FirstOrDefaultAsync(m => m.IdOferta == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // POST: Ofertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oferta = await _context.Ofertas.FindAsync(id);
            if (oferta != null)
            {
                _context.Ofertas.Remove(oferta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfertaExists(int id)
        {
            return _context.Ofertas.Any(e => e.IdOferta == id);
        }

		// Método para obtener el ID de la subasta dado el ID de la obra
		public int ObtenerIdSubasta(int idObra)
		{
			var idSubasta = _context.Subastas.FirstOrDefault(s => s.IdObra == idObra)?.IdSubasta ?? 0;
            System.Console.WriteLine("El valor del id de subasta es: " + idObra);
            return idSubasta;
		}

        // GET: Ofertas del usuario actual
        public async Task<IActionResult> MisOfertas()
        {
            // Obtener el ID del usuario actual desde la sesión
            var userId = HttpContext.Session.GetString("id");

            // Convertir el ID de usuario a entero
            var userIdInt = int.Parse(userId);

            // Obtener las ofertas del usuario actual
            var ofertas = await _context.Ofertas
                .Where(o => o.IdUsuarioComprador == userIdInt)
                .Include(o => o.IdSubastaNavigation)
                .Include(o => o.IdUsuarioCompradorNavigation)
                .ToListAsync();

            return View(ofertas);
        }




    }
}
