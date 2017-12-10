using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpiWebServer.GpioControls
{
    public class OutputGpioPin : GpioPin
    {
        public OutputGpioPin(GpioPinNumber pin) : base(pin, Direction.Out)
        {
            Write(State.Low);
        }

        public OutputGpioPin(GpioPinNumber pin, State state) : base(pin, Direction.Out)
        {
            Write(state);
        }

        public void Write(State state)
        {
            State = state;
            File.WriteAllText(GpioPath + GpioPinNumber.ToString().ToLower() + "/value", ((int)state).ToString());
        }

        public State State { get; private set; }
    }

}
