using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWUNED_EdgarArias_Proyecto2.Models;

namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{
    public class GaleriaController : Controller
    {
		private readonly Proyecto2Context _context;
		public GaleriaController(Proyecto2Context context)
		{
			_context = context;
		}

		public IActionResult Index()
        {

			// Obtener las obras de arte de la base de datos
			var obrasArte = _context.ObrasArtes.ToList();
			return View(obrasArte);
        }

        public IActionResult ObraIndividual(int idObra)
        {

			
			

			var obra = _context.ObrasArtes.Find(idObra);
			
			// Obtener una sola obra de arte de la base de datos
			if (obra == null)
            {
                return NotFound();
            }


			// Obtener la subasta correspondiente a la obra
			var subasta = _context.Subastas.FirstOrDefault(s => s.IdObra == idObra);

			var artista = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == obra.IdUsuario);




			// Pasar la obra y la subasta a la vista
			var viewModel = new ObraIndividual
			{
				Obra = obra,
				Subasta = subasta,
				Artista = artista
			};




			//abre la vista obra individual
			return View(viewModel);
        }







    }
}
