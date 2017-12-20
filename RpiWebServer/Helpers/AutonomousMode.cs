using RpiWebServer.Controllers;
using RpiWebServer.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RpiWebServer.Helpers
{
    public class AutonomousMode : ControllerHelper
    {
        public int defaultSafeDistanceInCm = 20;
        public bool safeDistance = true;
        public static bool externalCancelRequest = false;

        public AutonomousMode(IL298NMotorController iL298NMotorController, IUltraSonicDistanceSensor ultraSonicDistanceSensor)
            : base(iL298NMotorController, ultraSonicDistanceSensor)
        {
        }

        public void RunInAutoMode(int timeInMins)
        {
            Console.WriteLine("AutoPilot requested for mins : " + timeInMins);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (externalCancelRequest == false)
            {
                while (sw.Elapsed.TotalSeconds <= timeInMins * 60)
                {
                    Thread.Sleep(500);
                    Task t = Task.Run(() => GetDistance());
                    while (safeDistance == true){
                        //motorController.Forward();
                        MotorForward();
                        Thread.Sleep(1000);
                        if (externalCancelRequest == true){
                            safeDistance = false;
                            break;
                        }
                    }

                    if (externalCancelRequest == true){
                        safeDistance = false;
                        break;
                    }

                    //motorController.Stop();
                    MotorStop();
                    Thread.Sleep(1500);
                    //motorController.Right();
                    MotorRight();
                    Thread.Sleep(500);
                    //motorController.Forward();
                    MotorForward();
                    safeDistance = true;
                }

                if (externalCancelRequest == true){
                    break;
                }
            }
        }     

        private void GetDistance()
        {
            //var distance = ultraSonicDistanceSensor.GetDistanceFromSensor(); //2 to 500
            var distance = GetDistanceFromUDSCSensor(); //2 to 500
            if (distance >= defaultSafeDistanceInCm)
            {
                safeDistance = true;
            }
            
            while (distance >= defaultSafeDistanceInCm && safeDistance == true){
                Thread.Sleep(1000);
                distance = GetDistanceFromUDSCSensor();
                //distance = ultraSonicDistanceSensor.GetDistanceFromSensor();
                Console.WriteLine("Distance :" + distance);
            }

            safeDistance = false;
            Console.WriteLine("Obstacle Detected in {0} cm", distance);
        }
    }
}
