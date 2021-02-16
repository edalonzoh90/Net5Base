using BOT.DATA.Interfaces;
using BOT.DATA.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace BOT.DATA.Services
{
    public class SMSService : ISMSService
    {
        public bool Send(SmsModel model)
        {
            string result = "";
            var url = "https://www.calixtaondemand.com/Controller.php/__a/sms.send.remote.ol.sa?cte=42590&encpwd=aec1f8bc411c437034a301368d80f8f46e60c0b2a507b7d19a46ee57d38ab853&email=contacto3000mobile@cotemar.com.mx&auxiliar=Test001&mtipo=SMS";
            url = url + "&numtel=" + model.To + "&msg=" + model.Msg;

            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
            webrequest.Timeout = Int32.MaxValue;
            webrequest.Method = "POST";
            webrequest.ContentType = "application/x-www-form-urlencoded";

            using (var response = webrequest.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result.StartsWith("OK");
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}
