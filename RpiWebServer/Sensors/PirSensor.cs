using RpiWebServer.GpioControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RpiWebServer.Sensors
{
    //https://electrosome.com/pir-motion-sensor-hc-sr501-raspberry-pi/ 
    public class PirSensor :IDisposable
    {
        InputGpioPin inputPin;

        public PirSensor(int inputPinNo = 26)
        {
            inputPin = new InputGpioPin((GpioPinNumber)inputPinNo);
            Thread.Sleep(2000);
        }

        public bool CheckSensor()
        {
            if(inputPin.Read() == "1"){
                return true;
            }
            else{
                return false;
            }            
        }

        public void CleanUp()
        {
            inputPin.Cleanup();
        }

        public void Dispose()
        {
            CleanUp();
        }
    }
}
