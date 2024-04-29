using System;
using System.Linq;
using System.Transactions; // Importante para transacciones en Entity Framework


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWUNED_EdgarArias_Proyecto2.Models;
using PAWUNED_EdgarArias_Proyecto2.Services;

namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{
    public class FinalizarSubasta : Controller
    {
		private readonly IConfiguration _configuration;

		public IActionResult Index()
        {
            return View();
        }

		public FinalizarSubasta(IConfiguration configuration)
		{
			_configuration = configuration;
		}



		public void FinalizarSub(int idSubasta)
        {
            using (var dbContext = new Proyecto2Context())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Obtener la oferta más alta para la subasta
                        var ofertaMasAlta = dbContext.Ofertas
                            .Where(o => o.IdSubasta == idSubasta)
                            .OrderByDescending(o => o.MontoOferta)
                            .FirstOrDefault();

                        if (ofertaMasAlta == null)
                        {
                            // No hay ofertas para esta subasta, no se puede finalizar
                            throw new InvalidOperationException("No hay ofertas para esta subasta.");
                        }

						// Obtener al usuario ganador
						var usuarioGanador = dbContext.Usuarios
							.Where(u => u.IdUsuario == ofertaMasAlta.IdUsuarioComprador)
							.FirstOrDefault();

						if (usuarioGanador == null)
						{
							throw new InvalidOperationException("No se encontró al usuario ganador.");
						}


						// Obtener la obra de arte asociada a la oferta más alta
						var idSubastaOfertaMasAlta = dbContext.Ofertas
	                    .Where(o => o.IdSubasta == idSubasta)
	                    .OrderByDescending(o => o.MontoOferta)
	                    .Select(o => o.IdSubasta) // Seleccionar solo el Id de la subasta
	                    .FirstOrDefault();

						if (idSubastaOfertaMasAlta == 0)
						{
							throw new InvalidOperationException("No se encontraron ofertas para esta subasta.");
						}

						var idObraOfertaMasAlta = dbContext.Subastas
							.Where(subasta => subasta.IdSubasta == idSubastaOfertaMasAlta)
							.Select(subasta => subasta.IdObra)
							.FirstOrDefault();

						if (idObraOfertaMasAlta == 0)
						{
							throw new InvalidOperationException("No se encontró una obra de arte asociada a la oferta más alta.");
						}

						var obraArte = dbContext.ObrasArtes
							.Where(obra => obra.IdObra == idObraOfertaMasAlta)
							.FirstOrDefault();

						if (obraArte == null)
						{
							throw new InvalidOperationException("La obra de arte asociada a esta oferta no existe.");
						}

						// Actualizar la subasta con la oferta más alta, el usuario ganador y la fecha de cierre
						var subasta = dbContext.Subastas.Find(idSubasta);

                        if (subasta == null)
                        {
                            throw new InvalidOperationException("La subasta no existe.");
                        }

                        subasta.PrecioActual = ofertaMasAlta.MontoOferta;
                        subasta.FechaHoraCierre = DateTime.Now;
                        subasta.IdUsuarioGanador = ofertaMasAlta.IdUsuarioComprador;

						// Actualizar el estado de la obra de arte a "Vendida"
						obraArte.EstadoObra = "VENDIDA";

						dbContext.SaveChanges();

                        // Enviar correo electrónico al usuario ganador
                        var emailService = new EmailService(_configuration);
                        var cuerpoCorreo = "¡Felicidades! Has ganado la subasta" +
                                           "<p>¡Has ganado la subasta de la galería de arte PAW! </p>" +
                                           "<p>Obra: " + obraArte.TituloObra + "</p>"+
                                           "<p>Descripción: " + obraArte.DescripcionObra + "</p>" +
                                           "<p>Precio: " + ofertaMasAlta.MontoOferta + "</p>"+
                                           "<p>Para realizar el pago, dirígete al sitio. En la obra encontrarás un botón para realizar el pago por medio de PayPal o tarjeta.</p>" +
                                           "<p>Recuerda registrar el pago en el sitio para que el artista pueda entregar la obra.</p>";


						emailService.EnviarCorreo(usuarioGanador.Email, 
							"¡Felicidades! Has ganado la subasta",
                                    cuerpoCorreo);


						transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier excepción y deshacer la transacción si falla
                        transaction.Rollback();
                        throw new Exception("Error al finalizar la subasta: " + ex.Message);
                    }
                }
            }
        }

    }
}
