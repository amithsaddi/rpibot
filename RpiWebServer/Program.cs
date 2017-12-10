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
            //StartWebServer(args);

            //TestGpio();
            TestDistanceSensor();
            //TestPirSensor();
        }

        private static void GenerateSampleXml()
        {
            var actions = new List<PiAction>();
            actions.Add(new Model.PiAction() { Direction = "forward", Length = "24", Name = "Motor2" });
            actions.Add(new Model.PiAction() { Direction = "backward", Length = "24", Name = "Motor2" });

            DataModel dataModel = new DataModel();
            dataModel.Data = new Data() { PiActions = actions, Enable = false };
            XmlSerialiser xmlSerialiser = new XmlSerialiser();
            string data = xmlSerialiser.Serialize(dataModel);
            Console.WriteLine(data);
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

        private static void TestGpio()
        {
            var pin11 = new OutputGpioPin(GpioPinNumber.Gpio11);
            var pin2 = new OutputGpioPin(GpioPinNumber.Gpio17);

            var pin4 = new InputGpioPin(GpioPinNumber.Gpio4);
            pin4.Read();

            Console.WriteLine("Press ESC to stop");

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                pin11.Write(State.Low);
                Console.WriteLine("Pin 11 : " + pin11.State);
                pin2.Write(State.High);
                Console.WriteLine("Pin 2 : " + pin2.State);
                Thread.Sleep(750);
                pin11.Write(State.High);
                Console.WriteLine("Pin 11 : " + pin11.State);
                pin2.Write(State.Low);
                Console.WriteLine("Pin 2 : " + pin2.State);
                Thread.Sleep(750);
            }


            pin11.Cleanup();
            pin2.Cleanup();
        }

        private static void TestDistanceSensor()
        {
            UltraSonicDistanceSensor ultraSonicDistanceSensor = new UltraSonicDistanceSensor();

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                var distance = ultraSonicDistanceSensor.GetDistanceFromSensor();
                Console.WriteLine("Distance : " + distance);
                Thread.Sleep(1000);
            }

            ultraSonicDistanceSensor.Dispose();
        }

        private static void TestPirSensor()
        {
            PirSensor pirSensor = new PirSensor();

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                var detected = pirSensor.CheckSensor();
                Console.WriteLine("Motion Detected : " + detected);
                Thread.Sleep(1000);
            }

            pirSensor.Dispose();
        }
    }    
}
