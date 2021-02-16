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
    public class IDMService : IIDMService
    {
        private readonly IConfiguration configuration;
        private string IdmApiURL = "";

        public IDMService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.IdmApiURL = configuration.GetSection("AppSettings").GetSection("IdmAPI").Value;
        }

        public bool ResetPasswordAD(string username, string newPassword)
        {
            ResetPasswordModel model = new ResetPasswordModel() { Username = username, NewPassword = newPassword };

            string resp = Fetch<string>(new ApiRequest()
            {
                Uri = "Users/ChangePassAD",
                SData = JsonConvert.SerializeObject(model)
            });

            return !string.IsNullOrEmpty(resp);
        }

        public T Fetch<T>(ApiRequest req)
        {
            T resp = default(T);
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(IdmApiURL + req.Uri);

            webrequest.Timeout = req.TimeOut ?? Int32.MaxValue;
            webrequest.Method = req.Method ?? "POST";
            webrequest.ContentType = req.ContentType ?? "application/json";
            
            if (req.Uri != "authenticate")
                webrequest.Headers["Authorization"] = GetToken();

            byte[] byteArray = Encoding.UTF8.GetBytes(req.SData);
            webrequest.ContentLength = byteArray.Length;
            Stream dataStream = webrequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                using (var response = webrequest.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = reader.ReadToEnd();
                        resp = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                    }
                }
            }
            catch(Exception ex)
            {
                //Se agrega exclusivamente por el ChangePassAD, valdría la pena revisar con el equipo de idm si es la mejor opción esa respuesta
            }

            return resp;
        }

        public string GetToken()
        {
            //Es probable que haga falta implementar el refreshtoken, cuando se agreguen peticiones de usuarios 
            string TokenAcceso = configuration["IDM_Token"];
            string Vigencia = configuration["IDM_Expires"];
            string DateExpiredToken = configuration["IDM_DateExpired"];
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
                    Uri = "authenticate",
                    SData = configuration.GetSection("AppSettings").GetSection("IdmAPICredential").Value
                });

                var src = DateTime.Now;
                var Date = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, 0);

                configuration["IDM_Token"] = token.Access_token;
                configuration["IDM_Expires"] = token.Expires_in.ToString();
                configuration["IDM_DateExpired"] = Date.ToString();
            }

            return "bearer " + configuration["IDM_Token"];
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}
