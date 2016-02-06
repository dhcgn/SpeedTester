using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SpeedTester
{
    internal class Program
    {
        private static readonly string Location = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        private static readonly string LogFile = Path.Combine(Location, "log.txt");

        static void Main(string[] args)
        {
            if (args != null && args.Any())
            {
                Console.Out.WriteLine("You passed some arguments, this program doen't take any. It is only config driven, please see sample config.");
            }

            Print("Start");

            if (!SpeedTesterConfig.IsConfigSamplePresent())
            {
                Print("Creating sample config ...");
                SpeedTesterConfig.SaveSampleConfig();
            }

            var isConfigPresent = SpeedTesterConfig.IsConfigPresent();
            if (!isConfigPresent)
            {
                Print($"No config found at {SpeedTesterConfig.ConfigFullPath}, see smaple config.");
                Environment.ExitCode = -1;
                return;
            }

            Print("Loading config ...");
            var config = SpeedTesterConfig.Loadconfig();

            var bandwidthReports = new List<DownloadResult>();

            foreach (var download in config.Downloads)
            {
                Print($"WakeUp webserver for file: {download.Link}");
                var didRespond = WakeUpServer(download.Link);

                if (didRespond)
                {
                    var result = DoBandwidthTest(download);
                    bandwidthReports.Add(result);

                    Print($"Download for file {download.Link} completed in {result.TotalSeconds:0.000}s with a average bandwidth of {result.BandwidthMbitPerSecond:0.000} Mbit/s");
                    WriteLog(result, LogFile);
                }
                else
                {
                    Print($"Unable to download file: {download.Link}");
                }
            }

            Print($"Send email to {config.MailConfig.MailTo}");
            Mailer.SendMail(bandwidthReports, config.MailConfig, LogFile);

#if DEBUG
            Print("You are runnung the debug version, press any key to exit.");
            Console.ReadKey();
#endif
        }

        private static DownloadResult DoBandwidthTest(Download download)
        {
            var startTime = DateTime.UtcNow;

            var webClient = new WebClient();
            var length = webClient.DownloadData(download.Link).Length;

            var totalSeconds = (DateTime.UtcNow - startTime).TotalSeconds;

            return new DownloadResult()
            {
                TotalSeconds = totalSeconds,
                Length = length,
                BandwidthMbitPerSecond = length*8/totalSeconds/1024/1024,
                Download = download,
            };
        }

        private static bool WakeUpServer(string url)
        {
            try
            {
                WebRequest webRequest = HttpWebRequest.Create(url);
                webRequest.Method = "HEAD";
                HttpWebResponse webResponse = (HttpWebResponse) webRequest.GetResponse();

                return webResponse.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static void Print(string msg)
        {
            Console.Out.WriteLine($"{DateTime.Now.ToString("O")} - {msg}");
        }

        private static void WriteLog(DownloadResult result, string logFile)
        {
            try
            {
                File.AppendAllLines(logFile, new List<string>() {$"{DateTime.Now.ToString("s")};\"{result.Download.Name}\";\"{result.Download.Link}\";{result.BandwidthMbitPerSecond};{result.Length};{result.TotalSeconds}"});
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }
    }

    public class DownloadResult
    {
        public double TotalSeconds { get; set; }
        public int Length { get; set; }
        public double BandwidthMbitPerSecond { get; set; }
        public Download Download { get; set; }
    }
}