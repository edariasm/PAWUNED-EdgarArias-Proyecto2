using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System;
using PAWUNED_EdgarArias_Proyecto2.Services;


namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{ 

	public class EmailController : Controller
	{
		private readonly EmailService _emailService;

		public EmailController(EmailService emailService)
		{
			_emailService = emailService;
		}

		[HttpPost]
		public IActionResult EnviarCorreoAlGanador(string destinatario)
		{
			try
			{
				// Lógica para enviar el correo electrónico al usuario ganador
				_emailService.EnviarCorreo(destinatario, "¡Felicidades! Has ganado la subasta", "¡Has ganado la subasta! ¡Felicidades!");
				return Ok("Correo electrónico enviado al ganador");
			}
			catch (Exception ex)
			{
				return BadRequest("Error al enviar el correo electrónico al ganador: " + ex.Message);
			}
		
		}
	} 

}
