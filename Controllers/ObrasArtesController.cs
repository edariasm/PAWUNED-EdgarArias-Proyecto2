using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWUNED_EdgarArias_Proyecto2.Models;

namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{
    public class ObrasArtesController : Controller
    {
        private readonly Proyecto2Context _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ObrasArtesController(Proyecto2Context context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: ObrasArtes
        public async Task<IActionResult> Index()
        {
            var proyecto2Context = _context.ObrasArtes.Include(o => o.IdUsuarioNavigation);
            return View(await proyecto2Context.ToListAsync());
        }

        // GET: ObrasArtes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obrasArte = await _context.ObrasArtes
                .Include(o => o.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdObra == id);
            if (obrasArte == null)
            {
                return NotFound();
            }

            return View(obrasArte);
        }

        // GET: ObrasArtes/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: ObrasArtes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdObra,TituloObra,DescripcionObra,CategoriaObra,DimensionesObra,PrecioInicial,FechaInicioSubasta,DuracionSubasta,ImagenesObra,EstadoObra")] ObrasArte obrasArte)
        {

            // Obtener el IdUsuario almacenado en la sesión
            var userIDString = HttpContext.Session.GetString("id");
            int.TryParse(userIDString, out int userId);

            //var userId = HttpContext.Session.GetInt32("id");


            // Verificar si el IdUsuario es válido
            if (userId > 0)
            {
                // Asignar el IdUsuario obtenido al objeto ObrasArte
                obrasArte.IdUsuario = userId;
                Console.WriteLine("el id de usuario es: "+ userId);


                if (ModelState.IsValid)
                {

                    _context.Add(obrasArte);
                    await _context.SaveChangesAsync();
					return RedirectToAction("Index", "Galeria");
					//return RedirectToAction(nameof(Index));
                    
                }

                // Mostrar un mensaje de error personalizado
                ModelState.AddModelError(string.Empty, "Hubo un error al procesar la solicitud. Por favor, corrija los errores y vuelva a intentarlo.");
                // Coloca un punto de interrupción aquí para examinar el ModelState
                var invalidEntries = ModelState.Where(e => e.Value.ValidationState == ModelValidationState.Invalid);
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            else
            {
                Console.WriteLine("usurio con ID: " + userId + "ha provocado un error de validacion en la base de datos");
                //return RedirectToAction("Index", "Home"); // Redirigir a la página de inicio de sesión
            }



                ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", obrasArte.IdUsuario);
            return View(obrasArte);
            
        }



        // GET: ObrasArtes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obrasArte = await _context.ObrasArtes.FindAsync(id);
            if (obrasArte == null)
            {
                return NotFound();
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", obrasArte.IdUsuario);
            return View(obrasArte);
        }

        // POST: ObrasArtes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdObra,IdUsuario,TituloObra,DescripcionObra,CategoriaObra,DimensionesObra,PrecioInicial,FechaInicioSubasta,DuracionSubasta,ImagenesObra")] ObrasArte obrasArte)
        {
            if (id != obrasArte.IdObra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obrasArte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObrasArteExists(obrasArte.IdObra))
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
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", obrasArte.IdUsuario);
            return View(obrasArte);
        }

        // GET: ObrasArtes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obrasArte = await _context.ObrasArtes
                .Include(o => o.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdObra == id);
            if (obrasArte == null)
            {
                return NotFound();
            }

            return View(obrasArte);
        }

        // POST: ObrasArtes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obrasArte = await _context.ObrasArtes.FindAsync(id);
            if (obrasArte != null)
            {
                _context.ObrasArtes.Remove(obrasArte);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObrasArteExists(int id)
        {
            return _context.ObrasArtes.Any(e => e.IdObra == id);
        }


        [HttpPost]
        public string SubirImagen(IFormFile imagen)
        {
            if (imagen != null && imagen.Length > 0)
            {
                try
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "galeria");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imagen.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imagen.CopyTo(fileStream);
                    }

                    return "/galeria/" + uniqueFileName; // Devuelve la ruta de la imagen subida
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error que pueda ocurrir al guardar la imagen
                    throw new Exception("Error al subir la imagen: " + ex.Message);
                }
            }

            throw new Exception("No se proporcionó ninguna imagen.");
        }


    }
}
