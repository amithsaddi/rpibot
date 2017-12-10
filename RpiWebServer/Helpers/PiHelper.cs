using RpiWebServer.Model;
using System.Collections.Generic;
using System.Text;
using System;
using RpiWebServer.GpioControls;

namespace RpiWebServer.Helpers
{
    public class PiHelper
    {
        public static string ResultString = "Updated @ " +  DateTime.Now;
        public static Dictionary<int, OutputGpioPin> dictionary = new Dictionary<int, OutputGpioPin>();
        public static Dictionary<string, Direction> DirectionDictionary = new Dictionary<string, Direction> { { "In", Direction.In }, { "Out", Direction.Out } };
        public static Dictionary<string, State> StateDictionary = new Dictionary<string, State> { { "High", State.High }, { "Low", State.Low } };
       
        public static void AddData(DataModel dataModel)
        {
            foreach (var pin in dataModel.PinConfig.Pin)
            {
                var pinNo = int.Parse(pin.PinNumber);
                if (!dictionary.ContainsKey(pinNo)){
                    GpioPinNumber gpioPinNo = (GpioPinNumber)pinNo;
                    Direction direction = DirectionDictionary[pin.Direction];
                    State state = StateDictionary[pin.State];
                    if (direction == Direction.Out){
                        var outputPin = new OutputGpioPin(gpioPinNo);
                        dictionary.Add(pinNo, outputPin);
                    }
                }
            }

            Console.WriteLine("Add Count : " + dictionary.Count);
        }

        public void RemoveData(DataModel dataModel)
        {
            foreach (var pin in dataModel.PinConfig.Pin)
            {
                int pinNo = int.Parse(pin.PinNumber);
                if (dictionary.ContainsKey(pinNo))
                {
                    var kvp = dictionary[pinNo];
                    kvp.Cleanup();
                    dictionary.Remove(pinNo);
                }
            }
        }

        public static void ProcessData(DataModel dataModel)
        {
            ResultString = string.Empty;
            foreach (var pin in dataModel.PinConfig.Pin){
                var pinNo = int.Parse(pin.PinNumber);
                if (dictionary.ContainsKey(pinNo)){
                    var outputPin = dictionary[pinNo];
                    State state = StateDictionary[pin.State];
                    outputPin.Write(state);

                    //PrintStatus(outputPin);
                }
            }

            Console.WriteLine("Process Count : " + dictionary.Count);
        }

        public static void GetData()
        {
            ResultString = "Updated @ " + DateTime.Now;
            foreach (var item in dictionary){
                PrintStatus(item.Value);
            }                           
        }

        private static void PrintStatus(OutputGpioPin pin)
        {            
            StringBuilder sb = new StringBuilder();                       
            sb.AppendLine("----------Update----------");
            //sb.AppendLine("Pin Read: " + pin.Read());
            sb.AppendLine("Pin : " + pin.GpioPinNumber);
            sb.AppendLine("State : " + pin.State);
            sb.AppendLine("Direction : " + pin.Direction);

            ResultString = ResultString + sb.ToString();
            Console.WriteLine(ResultString);
        }

        public static void CleanUpAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------Clean Up----------");
            Console.WriteLine("Cleaned Pins");
            foreach (var item in dictionary){            
                item.Value.Cleanup();
                Console.WriteLine(item.Key + ",");
                sb.Append(item.Key + ",");
            }

            dictionary.Clear();
            ResultString = ResultString + sb.ToString();
        }
    }
}
