
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWUNED_EdgarArias_Proyecto2.Models.Paypal_Order;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using PAWUNED_EdgarArias_Proyecto2.Models;



namespace PAWUNED_EdgarArias_Proyecto2.Controllers
{
    public class PaypalOrderController : Controller
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public PaypalOrderController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }


        

        public IActionResult Index()
            {
            var paypalSettings = _configuration.GetSection("PayPal").Get<PayPalSettingsModel>();
            return View(paypalSettings);
        }


            [HttpPost]
            public async Task<IActionResult> ProcessPayment([FromBody] PaypalOrder paypalOrder)
            {
                try
                {
                var paypalSettings = _configuration.GetSection("PayPal").Get<PayPalSettingsModel>();
                
                // Ejemplo de solicitud a la API de PayPal usando HttpClient
                    var paypalApiUrl = _configuration["Paypal:ApiUrl"];
                    var paypalClientId = _configuration["Paypal:ClientId"];
                    var paypalSecret = _configuration["Paypal:Secret"];

                    // Construye la solicitud al endpoint de PayPal
                    var client = _clientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Access-Token");

                    // Serializa el objeto PaypalOrder a JSON
                    var paypalOrderJson = JsonConvert.SerializeObject(paypalOrder);

                    // Realiza la solicitud POST a la API de PayPal
                    var response = await client.PostAsync(paypalApiUrl, new StringContent(paypalOrderJson, Encoding.UTF8, "application/json"));

                    // Verifica el resultado de la solicitud
                    if (response.IsSuccessStatusCode)
                    {
                        // El pago se realizó con éxito
                        return Json(new { success = true, message = "El pago se realizó con éxito" });
                    }
                    else
                    {
                        // Hubo un error al procesar el pago
                        return Json(new { success = false, message = "Hubo un error al procesar el pago" });
                    }
                }
                catch (Exception ex)
                {
                    // Captura cualquier excepción que ocurra durante el procesamiento del pago
                    return Json(new { success = false, message = "Ocurrió un error al procesar el pago: " + ex.Message });
                }
            }
    }
}
