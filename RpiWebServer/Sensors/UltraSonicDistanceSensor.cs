using RpiWebServer.GpioControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RpiWebServer.Sensors
{
    //Works with 'HC-SR04 Sonar Module Distance Sensor'
    //https://stackoverflow.com/questions/30124861/ultrasonic-sensor-raspberry-pi-2-c-sharp-net
    public class UltraSonicDistanceSensor : IDisposable, IUltraSonicDistanceSensor
    {
        private long NoSignal;
        private long Signal;

        public OutputGpioPin OutputPin;
        public InputGpioPin inputPin;

        public UltraSonicDistanceSensor(int inputPinNo = 16, int OutputPinNo = 12)
        {
            OutputGpioPin OutputPin = new OutputGpioPin((GpioPinNumber)OutputPinNo);
            InputGpioPin inputPin = new InputGpioPin((GpioPinNumber)inputPinNo);
        }

        public double GetDistanceFromSensor(string measure = "cm")
        {
            Thread.Sleep(3);
            OutputPin.Write(State.High);
            Thread.Sleep(1);
            OutputPin.Write(State.Low);

            while (inputPin.Read() == "0"){
                NoSignal = DateTime.Now.Ticks;
            }
            while (inputPin.Read() == "1"){
                Signal = DateTime.Now.Ticks;
            }

            var tickDiff = Signal - NoSignal;
            double distance;
            if (measure == "cm"){
                distance = tickDiff / 0.000058;
            }
            else if (measure == "in"){
                distance = tickDiff / 0.000148;
            }
            else{
                Console.WriteLine("Improper choice of measurement: in or cm");
                distance = 0;
            }
           
            return distance;
        }

        public string GetPinStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Pin : {0}, Direction : {1} Status : {2} ", OutputPin.GpioPinNumber, OutputPin.Direction, OutputPin.State.ToString()));
            sb.AppendLine(string.Format("Pin : {0}, Direction : {1} Status : {2} ", inputPin.GpioPinNumber, inputPin.Direction, "N/A"));

            return sb.ToString();
        }
        public void CleanUp()
        {
            OutputPin.Cleanup();
            inputPin.Cleanup();
        }

        public void Dispose()
        {
            CleanUp();
        }
    }
}
