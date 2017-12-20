using System;
using System.Threading;

namespace RpiWebServer.Sensors
{
    public interface IUltraSonicDistanceSensor
    {
        void CleanUp();
        void Dispose();
        double GetDistanceFromSensor(string measure = "cm");
        string GetPinStatus();
    }

    public class DummyUltraSonicDistanceSensor : IUltraSonicDistanceSensor, IDisposable
    {
        public void CleanUp()
        {
            Console.WriteLine("Cleaned Pins 1, 2");
        }

        public void Dispose()
        {
            CleanUp();
        }

        public double GetDistanceFromSensor(string measure = "cm")
        {
            int val = 60 - DateTime.Now.Second;
            return double.Parse(val.ToString());
        }

        public string GetPinStatus()
        {
            string s = "Pin : 1 , State : Low, Direction : Out";
            return "Pin : 2 , State : Low, Direction : Out" + Environment.NewLine + s;
        }

        public void Foo()
        {            
            int currentSecond = 60 - DateTime.Now.Second;      //0 to 59       
        }
    }

}