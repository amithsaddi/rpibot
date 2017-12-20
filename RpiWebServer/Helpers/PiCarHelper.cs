using System;
using RpiWebServer.Model;
using System.Text;
using System.Net;
using System.IO;
using RpiWebServer.Controllers;
using RpiWebServer.Sensors;
using System.Threading.Tasks;

namespace RpiWebServer.Helpers
{    
    public class PiCarHelper : AutonomousMode
    {        
        public PiCarHelper(IL298NMotorController iL298NMotorController, IUltraSonicDistanceSensor ultraSonicDistanceSensor) : base(iL298NMotorController, ultraSonicDistanceSensor)
        {
           
        }               

        public string ResultString;

        public void RequestProcess(HttpListenerContext ctx)
        {
            ResultString = string.Empty;
            HttpListenerRequest httpListenerRequest = ctx.Request;
            var reader = new StreamReader(httpListenerRequest.InputStream);
            var streamString = reader.ReadToEnd();

            switch (httpListenerRequest.HttpMethod)
            {
                case "POST":
                    ProcessPOST(httpListenerRequest, streamString);
                    break;
                case "DELETE":

                    break;
                case "GET":
                    ProcessGET(httpListenerRequest);

                    //ResultString = SendResponse2(null);
                    break;
                default:
                    break;
            }

            //Console.WriteLine("Hash of motorController : " + motorController.GetHashCode());
            //Console.WriteLine("Hash of ultraSonicDistanceSensor : " + ultraSonicDistanceSensor.GetHashCode());
            //Console.WriteLine(ResultString);
        }
        private void ProcessPOST(HttpListenerRequest httpListenerRequest, string parameters = null)
        {
            switch (httpListenerRequest.RawUrl)
            {
                case "/1": //left
                case "/left":
                    MotorLeft();
                    break;
                case "/2": //forward
                case "/forward":
                    MotorForward();
                    break;
                case "/3":  //right
                case "/right":
                    MotorRight();
                    break;
                case "/4":  //reverse
                case "/backward":
                    MotorReverse();
                    break;
                case "/5":  //stop
                case "/stop":
                    MotorStop();
                    break;
                case "/automodeOn":
                case "/autoon":
                    if(parameters == null){
                        parameters = "3";
                    }
                    Console.WriteLine("Time :" + parameters);
                    AutonomousMode.externalCancelRequest = false;
                    Task t = Task.Run(() => RunInAutoMode(int.Parse(parameters)));
                    break;
                case "/automodeOff":
                case "/autooff":
                    AutonomousMode.externalCancelRequest = true;
                    Console.WriteLine("AutoPilot Request : Off");
                    break;
                default:
                    break;
            }
        }

        private void ProcessGET(HttpListenerRequest httpListenerRequest)
        {
            switch (httpListenerRequest.RawUrl)
            {                
                case "/distance":
                    //ResultString = DateTime.Now.Second.ToString();
                    //                    ResultString = ultraSonicDistanceSensor.GetDistanceFromSensor().ToString();
                    ResultString = GetDistanceFromUDSCSensor().ToString();
                    Console.WriteLine("Distance : " + ResultString);
                    break;
                case "/pinstatus":

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(SystemInfo());
                    sb.AppendLine("Pin Status");
                    sb.AppendLine(GetPinStatusFromMc());
                    sb.AppendLine(GetPinStatusFromUDSC());
                    //sb.AppendLine(motorController.GetPinsStatus());
                    //sb.AppendLine(ultraSonicDistanceSensor.GetPinStatus());
                    //ResultString = motorController.GetPinsStatus();
                    //ResultString = ResultString + ultraSonicDistanceSensor.GetPinStatus();
                    //Console.WriteLine(sb.ToString());

                    ResultString = sb.ToString();
                    break;
                default:
                    ResultString = BuildHtml();
                    break;
            }
        }

        private void DefaultSystem()
        {
            PiAction action = new PiAction() { Direction = "Forward", Length = "24", Name = "Motor1" };
            ResultString = string.Format("Default Motor @ '{3}'- Name : '{0}',  Direction : '{1}', Length = '{2}'", action.Name, action.Direction, action.Length, DateTime.Now);
            ResultString = ResultString + Environment.NewLine + SystemInfo();
        }

        private string SendResponse()
        {
            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
        }

        private string BuildHtml()
        {
            string text = File.ReadAllText(@"source/html/index3.html");
            return text;
        }

        private string SystemInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("OS Version : " + Environment.OSVersion);
            sb.AppendLine("MachineName : " + Environment.MachineName);
            sb.AppendLine("ProcessorCount : " + Environment.ProcessorCount);
            sb.AppendLine("CurrentDirectory : " + Environment.CurrentDirectory);
            sb.AppendLine("Processed TimeStamp : " + DateTime.Now);

            return sb.ToString();
        }

        public void CleanUp()
        {
            CleanAllControllersAndSensors();
            //motorController.CleanUp();
            //ultraSonicDistanceSensor.CleanUp();
        }
    }
}
