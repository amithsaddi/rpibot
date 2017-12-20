using RpiWebServer.GpioControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpiWebServer.Controllers
{
    public class L298NMotorController : IDisposable, IL298NMotorController
    {
        private readonly int motor1APinNo = 16;
        private readonly int motor1BPinNo = 18;
        private readonly int motor2APinNo = 11;
        private readonly int motor2BPinNo = 15;

        private OutputGpioPin motor1APin;
        private OutputGpioPin motor1BPin;
        private OutputGpioPin motor2APin;
        private OutputGpioPin motor2BPin;

        //int motor1APin = 16, int motor1BPin =18, int motor2APin=11, int motor2BPin =15
        public L298NMotorController(int motor1APin = 17, int motor1BPin =18, int motor2APin=23, int motor2BPin =24)
        {
            this.motor1APinNo = motor1APin;
            this.motor1BPinNo = motor1BPin;
            this.motor2APinNo = motor2APin;
            this.motor2BPinNo = motor2BPin;

            Initialize();
        }

        public void Initialize()
        {
           motor1APin = new OutputGpioPin((GpioPinNumber) motor1APinNo);
           motor1BPin = new OutputGpioPin((GpioPinNumber) motor1BPinNo);
           motor2APin = new OutputGpioPin((GpioPinNumber) motor2APinNo);
           motor2BPin = new OutputGpioPin((GpioPinNumber) motor2BPinNo);
        }

        public void Left()
        {
            motor1APin.Write(State.High);
            motor1BPin.Write(State.Low);
            motor2APin.Write(State.Low);
            motor2BPin.Write(State.High);
            Console.WriteLine("Left");
        }

        public void Right()
        {
            motor1APin.Write(State.Low);
            motor1BPin.Write(State.High);
            motor2APin.Write(State.High);
            motor2BPin.Write(State.Low);
            Console.WriteLine("Right");
        }

        public void Forward()
        {
            motor1APin.Write(State.Low);
            motor1BPin.Write(State.High);
            motor2APin.Write(State.Low);
            motor2BPin.Write(State.High);
            Console.WriteLine("Forward");
        }

        public void Reverse()
        {
            motor1APin.Write(State.High);
            motor1BPin.Write(State.Low);
            motor2APin.Write(State.High);
            motor2BPin.Write(State.Low);
            Console.WriteLine("Reverse");
        }

        public void Stop()
        {
            motor1APin.Write(State.Low);
            motor1BPin.Write(State.Low);
            motor2APin.Write(State.Low);
            motor2BPin.Write(State.Low);
            Console.WriteLine("Stop");
        }

        public void CleanUp()
        {
            motor1APin.Cleanup();
            motor1BPin.Cleanup();
            motor2APin.Cleanup();
            motor2BPin.Cleanup();
        }

        public void Dispose()
        {
            CleanUp();
        }

        public string GetPinsStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Pin : {0}, Direction : {1} Status : {2} ", motor1APin.GpioPinNumber, motor1APin.Direction, motor1APin.State.ToString()));
            sb.AppendLine(string.Format("Pin : {0}, Direction : {1} Status : {2} ", motor1BPin.GpioPinNumber, motor1BPin.Direction, motor1BPin.State.ToString()));
            sb.AppendLine(string.Format("Pin : {0}, Direction : {1} Status : {2} ", motor2APin.GpioPinNumber, motor2APin.Direction, motor2APin.State.ToString()));
            sb.AppendLine(string.Format("Pin : {0}, Direction : {1} Status : {2} ", motor2BPin.GpioPinNumber, motor2BPin.Direction, motor2BPin.State.ToString()));


            return sb.ToString();
        }
    }
}
