using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester
{
    public  class Mailer
    {
        public static void SendMail(List<DownloadResult> results, MailConfig mailConfig, string LogFile)
        {
            try
            {
                string subject = "Speedtest-Report from " + DateTime.Now.ToString(CultureInfo.GetCultureInfo("de-DE"));
                string body = CreateMailBody(results);

                MailMessage message = new MailMessage(mailConfig.MailFrom, mailConfig.MailTo, subject, body);
                message.Attachments.Add(new Attachment(LogFile));
                SmtpClient client = new SmtpClient(mailConfig.SmtpServer);
                client.Credentials = new NetworkCredential(mailConfig.MailFrom, mailConfig.Password);

                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }

        public static string CreateMailBody(List<DownloadResult> results)
        {
            var sb = new StringBuilder();

            foreach (var result in results)
            {
                sb.AppendLine($"{result.BandwidthMbitPerSecond:0.00} Mbit/s from {result.Download.Name}, link: {result.Download.Link}");
            }

            return sb.ToString();
        }
    }
}
