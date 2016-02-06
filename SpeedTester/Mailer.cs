using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SpeedTester
{
    public class Mailer
    {
        public static void SendMail(List<DownloadResult> results, MailConfig mailConfig, string LogFile)
        {
            try
            {
                string subject = "Speedtest-Report from " + DateTime.Now.ToString("s");
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
            sb.AppendLine($"{results.Select(result => result.BandwidthMbitPerSecond).Max():0.00} Mbit/s was fastest measured speed.");
            sb.AppendLine();
            sb.AppendLine($"This email contains the result of a bandwidth test startet on computer: {Environment.MachineName}. You receive this email because your adress was added.");
            sb.AppendLine($"Project papge: https://github.com/dhcgn/SpeedTester, please report issues to https://github.com/dhcgn/SpeedTester/issues");
            sb.AppendLine();

            foreach (var result in results.OrderByDescending(result => result.BandwidthMbitPerSecond))
            {
                sb.AppendLine($"{result.BandwidthMbitPerSecond:0.00} Mbit/s from {result.Download.Name}, link: {result.Download.Link}");
            }

            return sb.ToString();
        }
    }
}