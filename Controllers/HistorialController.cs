using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWUNED_EdgarArias_Proyecto2.Models;

namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{
    public class HistorialController : Controller
    {
        private readonly Proyecto2Context _context;

        public HistorialController(Proyecto2Context context)
        {
            _context = context;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Historial()
        {
            var userIdString = HttpContext.Session.GetString("id");
            if (userIdString != null && int.TryParse(userIdString, out int userId))
            {
                var tipoUsuario = HttpContext.Session.GetString("tipoUsuarioSesion");
                if (tipoUsuario != null)
                {
                    var historial = ObtenerHistorial(userId.ToString(), tipoUsuario);
                    return View(historial);
                }

                else
                {
                    // Manejo de error: redirigir a una página de error o realizar alguna otra acción apropiada
                    return RedirectToAction("Error");
                }
            }
            else
            {
                // Manejo de error: redirigir a una página de error o realizar alguna otra acción apropiada
                return RedirectToAction("Error");
            }

           
        }

        private IEnumerable<HistorialViewModel> ObtenerHistorial(string userId, string tipoUsuario)
        {
            // Lógica para obtener el historial
            var historial = new List<HistorialViewModel>();

            var userIdInt = int.Parse(userId);


            // Obtener las obras vendidas/compradas
            var obras = _context.ObrasArtes.Where(o => o.IdUsuario == userIdInt).ToList();


            foreach (var obra in obras)
            {
                var subastas = _context.Subastas.Where(s => s.IdObra == obra.IdObra).ToList();

                foreach (var subasta in subastas)
                {
                    var ofertas = _context.Ofertas.Where(o => o.IdSubasta == subasta.IdSubasta).ToList();

                    foreach (var oferta in ofertas)
                    {
                        var movimientos = new List<OfertaViewModel>
                {
                    new OfertaViewModel
                    {
                        IdUsuarioComprador = oferta.IdUsuarioComprador,
                        MontoOferta = oferta.MontoOferta
                    }
                };

                        var item = new HistorialViewModel
                        {
                            //TituloObra = obra.TituloObra,
                            idobra = obra.IdObra,

                            Precio = obra.PrecioInicial,
                            //FechaVentaCompra = obra.FechaInicioSubasta,
                            Movimientos = movimientos
                        };
                        historial.Add(item);
                    }
                }
            }
            return historial;


        }



    }
}
