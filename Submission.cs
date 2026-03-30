using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://maqboolmuslim.github.io/cse445-a4/NationalParks.xml";
        public static string xmlErrorURL = "https://maqboolmuslim.github.io/cse445-a4/NationalParksErrors.xml";
        public static string xsdURL = "https://maqboolmuslim.github.io/cse445-a4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string errorMessage = "No errors are found";
            try
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new System.IO.StringReader(DownloadContent(xsdUrl))));

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemas;
                settings.ValidationEventHandler += (sender, e) => { errorMessage = e.Message; };

                XmlReader reader = XmlReader.Create(new System.IO.StringReader(DownloadContent(xmlUrl)), settings);
                while (reader.Read()) { }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            return errorMessage;
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(DownloadContent(xmlUrl));
                return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}