using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http; //Nos va permitir utilizar las API's
using System.Net.Http.Headers;
using Newtonsoft.Json; //Nos permite convertir nuestras clases en objetos Json
using CapaEntidad.Paypal;
namespace CapaNegocio
{
    public class CN_Paypal
    {
        private static string urlpaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clientId = ConfigurationManager.AppSettings["ClientId"];
        private static string secret = ConfigurationManager.AppSettings["Secret"];

        //Crear un método que nos permita crear una solicitud de cobro en paypal
        public async Task<Respuesta_Paypal<RespuestaPago>>CrearSolicitud(OrdenPago orden)
        {
            Respuesta_Paypal<RespuestaPago> respuesta_paypal = new Respuesta_Paypal<RespuestaPago>();
            //Creamos una solicitud HttpClient
            using (var  client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);
                var authToken = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //Ejecutar la API
                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);
                //Leer la respuesta
                respuesta_paypal.Status = response.IsSuccessStatusCode;
                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;

                    RespuestaPago respuestaPago = JsonConvert.DeserializeObject<RespuestaPago>(jsonRespuesta);

                    respuesta_paypal.Response = respuestaPago;

                }
                return respuesta_paypal;

            }

        }


        public async Task<Respuesta_Paypal<CapturaRespuesta>> AprobarPago(string token)
        {
            Respuesta_Paypal<CapturaRespuesta> respuesta_paypal = new Respuesta_Paypal<CapturaRespuesta>();
            //Creamos una solicitud HttpClient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);
                var authToken = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                //Ejecutar la API agregando un parámetro: token
                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);
                //Leer la respuesta
                respuesta_paypal.Status = response.IsSuccessStatusCode;
                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;

                    CapturaRespuesta capturaRespuesta = JsonConvert.DeserializeObject<CapturaRespuesta>(jsonRespuesta);

                    respuesta_paypal.Response = capturaRespuesta;

                }
                return respuesta_paypal;

            }

        }


    }
}
