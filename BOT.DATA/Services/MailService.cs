using BOT.DATA.Interfaces;
using BOT.DATA.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace BOT.DATA.Services
{
    public class MailService : IMailService
    {

        private readonly IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Send(MailNotificationModel parameters)
        {
            var hostName = configuration.GetSection("AppSettings").GetSection("Mail_HostName").Value;
            var user = configuration.GetSection("AppSettings").GetSection("Mail_User").Value;
            var pass = configuration.GetSection("AppSettings").GetSection("Mail_Pass").Value;
            var endpoint = configuration.GetSection("AppSettings").GetSection("Mail_Endpoint").Value;

            MailDispatcher.ClientMessage mailClient = new MailDispatcher.ClientMessage();
            mailClient.message = new MailDispatcher.MessageModel();
            mailClient.message.To = parameters.to;
            mailClient.message.CC = parameters.cc ?? string.Empty;
            mailClient.message.Subject = parameters.subject;
            mailClient.message.Body = parameters.body;

            if (parameters.attachments != null && parameters.attachments.Count > 0)
            {
                List<MailDispatcher.MessageAttachments> attachments = new List<MailDispatcher.MessageAttachments>();
                foreach (var item in parameters.attachments)
                {
                    MailDispatcher.MessageAttachments newAttachment = new MailDispatcher.MessageAttachments();
                    newAttachment.Content = item.file.ToArray();
                    newAttachment.extension = item.extension;
                    newAttachment.name = item.fileName + " " + DateTime.Now.ToString("dd-MM-yyyy");
                    item.file.Dispose();
                    attachments.Add(newAttachment);
                }
                mailClient.message.MessageAttachments = (attachments != null && attachments.Count > 0) ? attachments.ToArray() : null;
            }

            mailClient.message.UserCreated = hostName;
            mailClient.message.IsHtmlBody = true;
            mailClient.message.NameClient = hostName;
            mailClient.message.IdClient = 1;

            var Binding = GetBindingForEndpoint();
            var Endpoint = new EndpointAddress(endpoint);

            MailDispatcher.QueueClient mailService = new MailDispatcher.QueueClient(Binding, Endpoint);
            mailService.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
            {
                CertificateValidationMode = X509CertificateValidationMode.None,
                RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
            };

            mailService.ClientCredentials.UserName.UserName = user;
            mailService.ClientCredentials.UserName.Password = pass;

            Task<MailDispatcher.ClientMessage> mailDispatcherRequest;
            using (OperationContextScope scope = new OperationContextScope(mailService.InnerChannel))
            {
                string credentials = EncodeBasicAuthenticationCredentials(user, pass);
                HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                request.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;

                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;
                mailDispatcherRequest = mailService.CreateMessageAsync(mailClient);
            }

            mailClient = await mailDispatcherRequest;
            if (!mailClient.Success)
                throw new Exception(mailClient.Error);
        }
        

        private static string EncodeBasicAuthenticationCredentials(string username, string password)
        {
            string credentials = username + ":" + password;
            var asciiCredentials = (from c in credentials
                                    select c <= 0x7f ? (byte)c : (byte)'?').ToArray();
            return Convert.ToBase64String(asciiCredentials);
        }

        public static System.ServiceModel.Channels.Binding GetBindingForEndpoint()
        {

            var httpsBinding = new BasicHttpsBinding { Name = "SAPBasicHttpsBinding" };
            httpsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            httpsBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Basic;
            httpsBinding.Security.Mode = BasicHttpsSecurityMode.Transport;
            httpsBinding.MaxBufferSize = 2147483647;
            httpsBinding.MaxReceivedMessageSize = 2147483647;
            httpsBinding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            httpsBinding.AllowCookies = true;

            return httpsBinding;
        }
    }
}
