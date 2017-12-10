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
    public class UltraSonicDistanceSensor : IDisposable
    {
        private static long NoSignal;
        private static long Signal;

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
