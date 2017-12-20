using RpiWebServer.Helpers;
using RpiWebServer.Model;
using RpiWebServer.Serializers;
using System;
using System.Collections.Generic;
using System.Threading;
using RpiWebServer.GpioControls;
using RpiWebServer.Sensors;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RpiWebServer.Controllers;

namespace RpiWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdLineParser = ParseArguments(args);
            StartWebServer(cmdLineParser);

            IL298NMotorController iL298NMotorController = new L298NMotorController();
            IUltraSonicDistanceSensor ultraSonicDistanceSensor = new UltraSonicDistanceSensor();
            AutonomousMode autonomousMode = new AutonomousMode(iL298NMotorController, ultraSonicDistanceSensor);
            //autonomousMode.RunInAutoMode(3);
            Task t = Task.Run(() => autonomousMode.RunInAutoMode(3));
            Thread.Sleep(60000);
            AutonomousMode.externalCancelRequest = true;

            //
            PiTest.TestDistanceSensor();
        }

        private static void StartWebServer(CmdLineParser cmdParser)
        {
            WebServer ws = null;
            IL298NMotorController iL298NMotorController = new L298NMotorController();
            IUltraSonicDistanceSensor ultraSonicDistanceSensor = new UltraSonicDistanceSensor();


            if (cmdParser != null){                
                ws = new WebServer(string.Format("http://{0}:{1}/", cmdParser.HostIp, cmdParser.HostPort), iL298NMotorController, ultraSonicDistanceSensor);
            }
            
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
            Console.WriteLine("Stopped gracefully. Press a key to quit.");
            Console.ReadKey();
        }

        private static CmdLineParser ParseArguments(string[] args)
        {
            Dictionary<string, string> dict = args.Select(s => s.Split(':')).ToDictionary(s => s[0].Replace("/", ""), s => s[1]);
            CmdLineParser cmdLineParser = CmdLineParser.DictionaryToObject<CmdLineParser>(dict);
            return cmdLineParser;
        }
    }    
}
