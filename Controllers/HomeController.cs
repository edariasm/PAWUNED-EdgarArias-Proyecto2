using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWUNED_EdgarArias_Proyecto2.Models;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Proyecto2Context _context;

        public HomeController(ILogger<HomeController> logger, Proyecto2Context context)
        {
            _logger = logger;
            _context = context;


        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //procedimiento para consultar la base de datos para realizar el login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            int? usuarioId = _context.Usuarios
                .Where(u => u.NombreUsuario == username && u.Password == password)
                .Select(u => (int?)u.IdUsuario)
                .FirstOrDefault();

            if (usuarioId != null)
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);


                // Usuario válido, realizar las acciones necesarias

                //variables de sesion

                var sesionid = usuarioId.ToString();
                var tipouser = usuario.TipoUsuario;
                var nombreUsuario = usuario.NombreUsuario;
                var nombreArtista = usuario.NombreArtista;

                if (usuario != null)
                {
                    HttpContext.Session.SetString("id", sesionid);

                }

                if (tipouser != null)
                {
                    HttpContext.Session.SetString("tipoUsuarioSesion", tipouser);
                }

                if (nombreUsuario != null)
                {
                    HttpContext.Session.SetString("nombreUsuario", nombreUsuario);
                }

                if (nombreArtista != null)
                {
                    HttpContext.Session.SetString("nombreArtista", nombreArtista);
                }
                



                var sesionId2 = HttpContext.Session.GetString("id");

                System.Console.WriteLine("El valor de la variable de sesión 'id' es: " + sesionId2);
                System.Console.WriteLine("El valor de la variable de sesión 'tipoUsuario' es: " + tipouser);
                //se requiere una variable de sesion que almacene el peso del usuario


                // Redirigir a la página principal
                Console.WriteLine("usurio encontrado" + usuario);
                Console.WriteLine("usuario con ID: " + sesionid);
                //var idusuario = usuario.IdUsuario;


                //return RedirectToAction("Index","Home");
                return RedirectToAction(actionName: "Privacy", "Home");
            }
            else
            {
                // Usuario inválido, manejar el caso en consecuencia
                HttpContext.Session.SetString("Error", "Error: usuario no encontrado" + username);

                // Redirigir de nuevo a la página de inicio de sesión
                //return RedirectToAction("Create", "Usuarios");
                //return RedirectToAction(default);
                return RedirectToAction(actionName: "Privacy", "Home");

            }

        }//fin del procedimiento de consulta




    }//fin de home controller
} //fin de namespace
