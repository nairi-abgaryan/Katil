using System;
using System.IO;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Katil.Data.Repositories.UnitOfWork;
using Katil.Messages.EmailNotification.Events;

namespace Katil.Services.EmailNotification.EmailNotificationService.IntegrationEvents.EventHandling
{
    public class EmailNotificationIntegrationEventHandler : IConsumeAsync<EmailNotificationIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;

        public EmailNotificationIntegrationEventHandler(IBus bus, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "EmailNotification")]
        public async System.Threading.Tasks.Task ConsumeAsync(EmailNotificationIntegrationEvent message)
        {
            Console.WriteLine("Email Notification Integration Event Received: {0}, {1}", message.DisputeGuid, message.MessageType);

            try
            {
                var retValue = await SendMail(message);
            }
            catch (Exception)
            {
                throw;
            }

            Console.WriteLine("Email Notification Integration Event Archived: {0}, {1}", message.DisputeGuid, message.MessageType);
        }

        private async System.Threading.Tasks.Task<bool> SendMail(EmailNotificationIntegrationEvent message)
        {
            using (SmtpClient client = new SmtpClient())
            {
                if (!string.IsNullOrWhiteSpace(message.EmailTo))
                {
                    try
                    {
                        var mimeMessage = new MimeMessage();
                        mimeMessage.From.Add(new MailboxAddress(message.Title, message.EmailFrom));
                        mimeMessage.To.Add(new MailboxAddress(message.EmailTo));

                        message.Subject = message.Subject;

                        var builder = new BodyBuilder();

                        builder.TextBody = message.Body;
                        builder.HtmlBody = message.Body;

                        if (message.EmailAttachments != null)
                        {
                            foreach (var item in message.EmailAttachments)
                            {
                                var commonFilePath = GetCommonFilePath(item.CommonFileName);

                                using (Stream fs = new FileStream(commonFilePath, FileMode.Open))
                                {
                                    builder.Attachments.Add(item.CommonFileName, fs);
                                }
                            }
                        }

                        mimeMessage.Body = builder.ToMessageBody();

                        var host = GetSetting(SettingKeys.SmtpClientHost).Result.Value;
                        var port = int.Parse(GetSetting(SettingKeys.SmtpClientPort).Result.Value);
                        var enableSsl = bool.Parse(GetSetting(SettingKeys.SmtpClientEnableSsl).Result.Value);
                        var timeout = int.Parse(GetSetting(SettingKeys.SmtpClientTimeout).Result.Value);
                        var user = GetSetting(SettingKeys.SmtpClientUsername).Result.Value;
                        var encryptedPassword = GetSetting(SettingKeys.SmtpClientPassword).Result.Value;
                        var password = HashHelper.DecryptPassword(encryptedPassword, user);

                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Timeout = timeout;

                        client.Connect(host, port, SecureSocketOptions.Auto);

                        if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
                        {
                            await client.AuthenticateAsync(user, password);
                        }

                        await client.SendAsync(mimeMessage);

                        client.Disconnect(true);
                    }
                    catch (ServiceNotConnectedException serviceNotConnectedException)
                    {
                        Console.WriteLine(serviceNotConnectedException.Message + Environment.NewLine + "Failed to deliver message to " + message.EmailTo);
                        throw;
                    }
                    catch (ProtocolException protocolException)
                    {
                        Console.WriteLine(protocolException.Message + Environment.NewLine + "Failed to deliver message");
                        throw;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message + Environment.NewLine + "Unknown error");
                        throw;
                    }
                }

                return true;
            }
        }

        private async Task<SystemSettings> GetSetting(string key)
        {
            var setting = await _unitOfWork.SystemSettingsRepository.GetSetting(key);
            return setting;
        }

        private string GetFilePath(string filePath)
        {
            var rootFileFolder = GetSetting(SettingKeys.FileStorageRoot).Result.Value;
            return Path.Combine(rootFileFolder, filePath);
        }

        private string GetCommonFilePath(string filePath)
        {
            var rootFileFolder = GetSetting(SettingKeys.CommonFileStorageRoot).Result.Value;
            return Path.Combine(rootFileFolder, filePath);
        }
    }
}
