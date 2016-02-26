using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SpeedTester
{
    public class SpeedTesterConfig
    {
        private static readonly string Location = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        /// <summary>
        /// see: https://kb.leaseweb.com/display/KB/Link+speeds
        /// see: http://speedtest.tele2.net/
        /// see: http://speedtest.netcologne.de/
        /// see: http://speedtest-ams2.digitalocean.com/
        /// see: http://speedtest.encoline.de/
        /// </summary>
        public List<Download> Downloads { get; set; }

        public MailConfig MailConfig { get; set; }

        [XmlIgnore]
        public static string ConfigFullPath => Path.Combine(Location, "config.xml");

        [XmlIgnore]
        public static string ConfigSampleFullPath => Path.Combine(Location, "config.sample.xml");

        public static bool IsConfigPresent()
        {
            return File.Exists(ConfigFullPath);
        }

        public static bool IsConfigSamplePresent()
        {
            return File.Exists(ConfigSampleFullPath);
        }

        public static void SaveSampleConfig()
        {
            var sampleConfig = new SpeedTesterConfig()
            {
                Downloads = new List<Download>()
                {
                    new Download()
                    {
                        Name = "Digital Ocean - AMS - Mirror 2",
                        Link = "http://speedtest-ams2.digitalocean.com/10mb.test"
                    },
                    new Download()
                    {
                        Name = "Digital Ocean - AMS - Mirror 3",
                        Link = "http://speedtest-ams3.digitalocean.com/10mb.test"
                    },
                    new Download()
                    {
                        Name = "Digital Ocean - FRA",
                        Link = "http://speedtest-fra1.digitalocean.com/10mb.test"
                    },
                    new Download()
                    {
                        Name = "NetCologne - FRA",
                        Link = "http://speedtest.netcologne.de/test_10mb.bin"
                    },
                    new Download()
                    {
                        Name = "Tele2 - FRA",
                        Link = "http://speedtest.tele2.net/100MB.zip"
                    },
                    new Download()
                    {
                        Name = "Tele2 - AMS",
                        Link = "http://ams-speedtest-1.tele2.net/100MB.zip"
                    },
                    new Download()
                    {
                        Name = "leaseweb.com - AMS",
                        Link = "http://mirror.nl.leaseweb.net/speedtest/10mb.bin"
                    },
                    new Download()
                    {
                        Name = "leaseweb.com - FRA",
                        Link = "http://mirror.de.leaseweb.net/speedtest/10mb.bin"
                    },
                    new Download()
                    {
                        Name = "leaseweb.com - NY",
                        Link = "http://mirror.us.leaseweb.net/speedtest/10mb.bin"
                    },
                    new Download()
                    {
                        Name = "encoline.de - DE",
                        Link = "http://speedtest.encoline.de/10MB.bin"
                    },
                },
                MailConfig = new MailConfig()
                {
                    SmtpServer = "smtp.myserver.com",
                    MailFrom = "noreply@myserver.com",
                    MailTo = "John.Vulcano.32323@gmail.com",
                    Password = "secret",
                    EnableSsl = false
                }
            };
            Save(sampleConfig, ConfigSampleFullPath);
        }

        public static void Save(SpeedTesterConfig config, string path)
        {
            var xmlSerial = new XmlSerializer(config.GetType());
            Stream stream = new FileStream(path, FileMode.Create);
            xmlSerial.Serialize(stream, config);
        }

        public static SpeedTesterConfig Loadconfig()
        {
            var xmlSerial = new XmlSerializer(typeof (SpeedTesterConfig));
            Stream stream = new FileStream(ConfigFullPath, FileMode.Open);
            return xmlSerial.Deserialize(stream) as SpeedTesterConfig;
        }
    }

    public class MailConfig
    {
        public string SmtpServer { get; set; }
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class Download
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string Link { get; set; }
    }
}