using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpiWebServer.GpioControls
{
    public abstract class GpioPin
    {
        protected const string GpioPath = "/sys/class/gpio/";

        protected GpioPin(GpioPinNumber pin, Direction direction)
        {
            GpioPinNumber = pin;
            Direction = direction;

            File.WriteAllText(GpioPath + "export", ((int)pin).ToString());
            File.WriteAllText(GpioPath + pin.ToString().ToLower() + "/direction", direction.ToString().ToLower());
        }

        public GpioPinNumber GpioPinNumber { get; private set; }
        public Direction Direction { get; private set; }

        public string Read()
        {
            return File.ReadAllText(GpioPath + GpioPinNumber.ToString().ToLower() + "/value");
        }

        public void Cleanup()
        {
            File.WriteAllText(GpioPath + "unexport", ((int)GpioPinNumber).ToString());
        }
    }

    public enum GpioPinNumber
    {
        Gpio2 = 2,
        Gpio3,
        Gpio4,
        Gpio5,
        Gpio6,
        Gpio7,
        Gpio8,
        Gpio9,
        Gpio10,
        Gpio11,
        Gpio12,
        Gpio13,
        Gpio14,
        Gpio15,
        Gpio16,
        Gpio17,
        Gpio18,
        Gpio19,
        Gpio20,
        Gpio21,
        Gpio22,
        Gpio23,
        Gpio24,
        Gpio25,
        Gpio26,
        Gpio27,
    }

    public enum Direction
    {
        In,
        Out
    }

    public enum State
    {
        Low,
        High
    }
}
