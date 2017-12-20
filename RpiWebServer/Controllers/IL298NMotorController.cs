using System;
using System.Text;

namespace RpiWebServer.Controllers
{
    public interface IL298NMotorController
    {
        void CleanUp();
        void Dispose();
        void Forward();
        string GetPinsStatus();
        void Initialize();
        void Left();
        void Reverse();
        void Right();
        void Stop();
    }

    public class DummyMotorController : IL298NMotorController
    {
        public void CleanUp()
        {
            Console.WriteLine("Cleaned Pins 2, 3,4,5");
        }

        public void Dispose()
        {
            CleanUp();
        }

        public void Forward()
        {
            Console.WriteLine("Forward");
        }

        public string GetPinsStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Pin : 2 , State : Low, Direction : Out");
            sb.AppendLine("Pin : 3 , State : Low, Direction : Out");
            sb.AppendLine("Pin : 4 , State : Low, Direction : Out");
            sb.AppendLine("Pin : 5 , State : Low, Direction : Out");
            return sb.ToString();
        }

        public void Initialize()
        {
            Console.WriteLine("Initialize");
        }

        public void Left()
        {
            Console.WriteLine("Left");
        }

        public void Reverse()
        {
            Console.WriteLine("Reverse");
        }

        public void Right()
        {
            Console.WriteLine("Right");
        }

        public void Stop()
        {
            Console.WriteLine("Stop");
        }
    }
}