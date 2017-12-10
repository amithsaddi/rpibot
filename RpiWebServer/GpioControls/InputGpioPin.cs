using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpiWebServer.GpioControls
{
    public class InputGpioPin : GpioPin
    {
        public InputGpioPin(GpioPinNumber pin) : base(pin, Direction.In)
        {
        }
    }
}
