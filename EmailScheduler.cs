using EmailScheduler.Model;
using EmailScheduler.Model.Helper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace EmailScheduler
{
    public class EmailScheduler : BackgroundService
    {
        private readonly IOptions<AppSetting> _config;
        private IWebHostEnvironment _hostEnvironment;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public EmailScheduler(IServiceScopeFactory serviceScopeFactory, IWebHostEnvironment hostEnvironment, IOptions<AppSetting> config)
        {
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
            _hostEnvironment = hostEnvironment;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                List<int> messageList = new List<int>();
                List<int> messageRange = Enumerable.Range(1, EmailMessages().Count()).ToList();
                var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

                var randomNumber = new Random();
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    int index = randomNumber.Next(0, messageRange.Count);
                    messageList.Add(messageRange[index]);
                    messageRange.RemoveAt(index);

                    var messageId = messageList.Last();

                    foreach (var item in GetUsers())
                    {
                        EmailMessages _email = EmailMessages().Where(a=>a.MessageId==messageId).FirstOrDefault();
                        SendMail(_email.Subject, _email.Body, item.Email);
                    }
                    if (messageList.Count == EmailMessages().Count())
                    {
                        timer.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<EmailMessages> EmailMessages()
        {
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "Data/"+_config.Value.MessageFile);
            string messageDataJson = System.IO.File.ReadAllText(path);
            List<EmailMessages> emailMessage = JsonConvert.DeserializeObject<List<EmailMessages>>(messageDataJson);
            return emailMessage;
        }
        public List<User> GetUsers()
        {
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "Data/"+_config.Value.UserFile);
            string userDataJson = System.IO.File.ReadAllText(path);
            List<User> userData = JsonConvert.DeserializeObject<List<User>>(userDataJson);
            return userData;
        }
        public void SendMail(string subject,string body,string mailTo)
        {
            try
            {
                string smtpAddress = _config.Value.Smtp;
                int portNumber = _config.Value.Port;
                bool enableSSL = true;
                string emailFromAddress = _config.Value.EmailFrom; 
                string password = _config.Value.Password; 
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(mailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
          
        }
    }
}
