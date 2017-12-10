using RpiWebServer.Helpers;
using RpiWebServer.Model;
using RpiWebServer.Serializers;
using System;
using System.Collections.Generic;
using System.Threading;
using RpiWebServer.GpioControls;
using RpiWebServer.Sensors;

namespace RpiWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            StartWebServer(args);

            //PiTest.TestGpio();
            //PiTest.TestDistanceSensor();
            //PiTest.TestPirSensor();
        }
        
        private static void StartWebServer(string[] args)
        {
            WebServer ws;
            if (args.Length == 0) {
                ws = new WebServer(string.Format("http://localhost:8000/index/"));
            }
            else {
                string ip = args[0];
                string port = args[1];
                ws = new WebServer(string.Format("http://{0}:{1}/index/", ip, port));
            }
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
            PiHelper.CleanUpAll();
        }
    }    
}
