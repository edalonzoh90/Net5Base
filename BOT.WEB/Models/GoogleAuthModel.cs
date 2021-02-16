using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
//using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace BOT.WEB.Models
{
    public class GoogleAuthModel
    {
        public bool ExisteToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        private const string DriveClientSecretPath = @"~/Google/Api/Drive_client-secret.json";


        //public static void loadAppSecrets()
        //{
        //    try
        //    {
        //        using (var stream =
        //               new FileStream(HttpContext.Current.Server.MapPath(DriveClientSecretPath), FileMode.Open, FileAccess.Read))
        //        {
        //            //Helpers.SessionVar.AppGoogleSecrets = GoogleClientSecrets.Load(stream).Secrets;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static ClientSecrets getAppSecrets()
        //{
        //    try
        //    {
        //        using (var stream =
        //               new FileStream(HttpContext.Current.Server.MapPath(DriveClientSecretPath), FileMode.Open, FileAccess.Read))
        //        {
        //            return GoogleClientSecrets.Load(stream).Secrets;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //public GoogleAuthModel()
        //{
        //    ExisteToken = false;
        //    ClientId = string.Empty;
        //    ClientSecret = string.Empty;

        //    #region CONFIGURACIÓN DE AUTENTIFICACIÓN POR GOOGLE

        //    string jsonPath = HttpContext.Current.Server.MapPath(@"~/Google/Client/ClientLogin.json");

        //    if (File.Exists(jsonPath))
        //    {
        //        using (StreamReader r = new StreamReader(jsonPath))
        //        {
        //            string json = r.ReadToEnd();
        //            dynamic token = JsonConvert.DeserializeObject(json);
        //            ClientId = token.web.client_id.ToString();
        //            ClientSecret = token.web.client_secret.ToString();
        //        }

        //        if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(ClientSecret))
        //        {
        //            ExisteToken = true;
        //        }
        //    }

        //    #endregion CONFIGURACIÓN DE AUTENTIFICACIÓN POR GOOGLE
        //}
    }
}
