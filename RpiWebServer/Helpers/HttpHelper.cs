using RpiWebServer.Model;
using RpiWebServer.Serializers;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace RpiWebServer.Helpers
{
    public class HttpHelper
    {
        public static string ResultString;
        PiHelper piHelper = new PiHelper();

        private static DataModel DeserializeHttpRequest(string text)
        {
            ISerialiser serialiser = new XmlSerialiser();
            var dataModel = serialiser.Deserialize<DataModel>(text);
            return dataModel;
        }

        public static void RequestProcess(HttpListenerContext ctx)
        {
            ResultString = string.Empty;
            HttpListenerRequest httpListenerRequest = ctx.Request;
            var reader = new StreamReader(httpListenerRequest.InputStream);
            var streamString = reader.ReadToEnd();
            Console.WriteLine("Data received:" + streamString);            

            switch (httpListenerRequest.HttpMethod)
            {
                case "POST":
                    var contentType = httpListenerRequest.ContentType;
                    var dataModel = DeserializeHttpRequest(streamString);
                    PiHelper.AddData(dataModel);
                    PiHelper.ProcessData(dataModel);
                    PiHelper.GetData();
                    break;
                case "DELETE":
                    PiHelper.CleanUpAll();
                    break;
                case "GET":
                    PiHelper.GetData();                    
                    break;
                default:
                    break;
            }
            
            ResultString = SystemInfo() + Environment.NewLine + PiHelper.ResultString;
            Console.WriteLine(ResultString);
        }
               
        private static void DefaultSystem()
        {
            PiAction action = new PiAction() { Direction = "Forward", Length = "24", Name = "Motor1" };
            ResultString = string.Format("Default Motor @ '{3}'- Name : '{0}',  Direction : '{1}', Length = '{2}'", action.Name, action.Direction, action.Length, DateTime.Now);
            ResultString = ResultString + Environment.NewLine + SystemInfo();
        }

        private static string SendResponse()
        {
            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
        }

        private static string SendResponse2(HttpListenerRequest request)
        {
            //string text = File.ReadAllText(@"source/html/index.html");
            string text = File.ReadAllText(@"source/html/index.html");
            return text;
            //return string.Format(text);
        }

        private static string SystemInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("OS Version : " + Environment.OSVersion);
            sb.AppendLine("MachineName : " + Environment.MachineName);
            sb.AppendLine("ProcessorCount : " + Environment.ProcessorCount);
            sb.AppendLine("CurrentDirectory : " + Environment.CurrentDirectory);
            sb.AppendLine("Processed TimeStamp : " + DateTime.Now);
            
            return sb.ToString();
        }
    }   
}
