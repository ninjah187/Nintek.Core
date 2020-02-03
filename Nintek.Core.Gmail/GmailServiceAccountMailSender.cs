using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ContentType = System.Net.Mime.ContentType;

namespace Nintek.Core.Gmail
{
    public class GmailServiceAccountMailSender : IMailSender
    {
        // ATTENTION: in case of bugs, try this scope first
        // static readonly string[] Scopes = { @"https://mail.google.com/" };

        static readonly string[] DefaultScopes = { GmailService.Scope.GmailSend };

        readonly string _applicationName;
        readonly string _serviceAccountEmail;
        readonly string _certificateFilePath;
        readonly string[] _scopes;

        public GmailServiceAccountMailSender(
            string applicationName,
            string serviceAccountEmail,
            string certificateFilePath,
            string[] scopes = null)
        {
            _applicationName = applicationName;
            _serviceAccountEmail = serviceAccountEmail;
            _certificateFilePath = certificateFilePath;
            _scopes = scopes ?? DefaultScopes;
        }

        public async Task Send(
            string fromAddress,
            string toRecipients,
            string subject,
            string body = "",
            bool isBodyHtml = false)
        {
            var message = CreateMessage(fromAddress, toRecipients, subject, body, isBodyHtml);

            var certificate = new X509Certificate2(_certificateFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(_serviceAccountEmail)
            {
                Scopes = _scopes,
                User = fromAddress
            }.FromCertificate(certificate));

            using (var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            }))
            {
                var request = service.Users.Messages.Send(message, fromAddress);
                await request.ExecuteAsync();
            }
        }

        // not needed now; for future reference
        // mailMessage.Attachments.Add(attachement);
        static Message CreateMessage(
            string fromAddress,
            string toRecipients,
            string subject,
            string body = "",
            bool isBodyHtml = false)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                IsBodyHtml = isBodyHtml,
                BodyEncoding = Encoding.UTF8
            };
            mailMessage.To.Add(toRecipients);
            mailMessage.ReplyToList.Add(fromAddress);

            var htmlView = AlternateView.CreateAlternateViewFromString(body, isBodyHtml
                ? ContentTypes.Html
                : ContentTypes.Plain);
            htmlView.ContentType.CharSet = Encoding.UTF8.WebName;
            mailMessage.AlternateViews.Add(htmlView);

            var mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);

            var gmailMessage = new Message
            {
                Raw = Encode(mimeMessage.ToString())
            };

            return gmailMessage;
        }

        static string Encode(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert
                .ToBase64String(bytes)
                .Replace("+", "-")
                .Replace('/', '_')
                .Replace("=", "");
        }

        static class ContentTypes
        {
            public static ContentType Html { get; } = new ContentType("text/html");
            public static ContentType Plain { get; } = new ContentType("text/plain");
        }
    }
}
