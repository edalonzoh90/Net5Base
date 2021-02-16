using BOT.DATA.Helper;
using BOT.DATA.Interfaces;
using BOT.DATA.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BOT.DATA.Services
{
    public class HubApiService : IHubApiService
    {
        private readonly IConfiguration configuration;
        private string HubApiURL = "";

        public HubApiService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.HubApiURL = configuration.GetSection("AppSettings").GetSection("HubAPI").Value;
        }

        public EmpleadoModel GetDataEmployee(string ficha)
        {
            EmpleadoModel model = new EmpleadoModel() { Ficha = ficha };

            model = Fetch<ApiResponse<EmpleadoModel>>(new ApiRequest()
            {
                Uri = "api/Servicios/ObtenerDatosEmpleado",
                SData = JsonConvert.SerializeObject(model)
            }).Data;


            return model;
        }

        public DatosAreaEmpleadoModelo GetDataEmployeeArea(string ficha)
        {
            DatosAreaEmpleadoModelo model = new DatosAreaEmpleadoModelo() { Ficha = ficha };

            model = Fetch<ApiResponse<DatosAreaEmpleadoModelo>>(new ApiRequest()
            {
                Uri = "api/Servicios/ObtenerDatosAreaEmpleado",
                SData = JsonConvert.SerializeObject(model)
            }).Data;


            return model;
        }

        public T Fetch<T>(ApiRequest req)
        {
            T resp = default(T);
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(HubApiURL + req.Uri);

            webrequest.Timeout = req.TimeOut ?? Int32.MaxValue;
            webrequest.Method = req.Method ?? "POST";
            webrequest.ContentType = req.ContentType ?? "application/json";
            
            if (req.Uri != "auth/token")
                webrequest.Headers["Authorization"] = GetToken();

            byte[] byteArray = Encoding.UTF8.GetBytes(req.SData);
            webrequest.ContentLength = byteArray.Length;
            Stream dataStream = webrequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            using (var response = webrequest.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();
                    resp = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                }
            }

            return resp;
        }

        public string GetToken()
        {
            string TokenAcceso = configuration["HUB_Token"];
            string Vigencia = configuration["HUB_Expires"];
            string DateExpiredToken = configuration["HUB_DateExpired"];
            bool existValidToken = false;

            if (!string.IsNullOrEmpty(TokenAcceso) && !string.IsNullOrEmpty(Vigencia))
            {
                int Minutos = Convert.ToInt32(Vigencia) / 60;
                var src = DateTime.Now;
                var FechaActual = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, 0);
                DateTime hm = Convert.ToDateTime(DateExpiredToken).AddMinutes(Minutos);
                existValidToken = hm > FechaActual;
            }

            if (!existValidToken)
            {
                Token token = Fetch<Token>(new ApiRequest()
                {
                    Uri = "auth/token",
                    ContentType = "application/x-www-form-urlencoded",
                    SData = configuration.GetSection("AppSettings").GetSection("HubAPICredential").Value
                });

                var src = DateTime.Now;
                var Date = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, 0);

                configuration["HUB_Token"] = token.Access_token;
                configuration["HUB_Expires"] = token.Expires_in.ToString();
                configuration["HUB_DateExpired"] = Date.ToString();
            }

            return "bearer " + configuration["HUB_Token"];
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}
