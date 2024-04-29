using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace PAWUNED_EdgarArias_Proyecto2.Services
{
	public class EmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void EnviarCorreo(string destinatario, string asunto, string cuerpo)
		{
			var smtpServer = _configuration["SmtpConfiguration:Server"];
			var smtpPort = int.Parse(_configuration["SmtpConfiguration:Port"]);
			var smtpUsername = _configuration["SmtpConfiguration:Username"];
			var smtpPassword = _configuration["SmtpConfiguration:Password"];
			var senderName = _configuration["SmtpConfiguration:SenderName"];
			var senderEmail = _configuration["SmtpConfiguration:SenderEmail"];

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(senderName, senderEmail));
			message.To.Add(new MailboxAddress("", destinatario)); 
			message.Subject = asunto;
			message.Body = new TextPart("HTML")
			{
				Text = cuerpo
			};

			using (var client = new SmtpClient())
			{
				client.Connect(smtpServer, smtpPort, useSsl: false); 
				client.Authenticate(smtpUsername, smtpPassword);
				client.Send(message);
				client.Disconnect(true);
			}
		}

	}
}
