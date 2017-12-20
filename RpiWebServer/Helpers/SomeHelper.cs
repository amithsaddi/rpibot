using RpiWebServer.Controllers;
using RpiWebServer.Sensors;

namespace RpiWebServer.Helpers
{
    public class ControllerHelper
    {
        private IL298NMotorController l298NMtorController;
        private IUltraSonicDistanceSensor ultraSonicDistanceSensor;
       
        public ControllerHelper(IL298NMotorController motorController, IUltraSonicDistanceSensor _ultraSonicDistanceSensor)
        {
            this.l298NMtorController = motorController;
            this.ultraSonicDistanceSensor = _ultraSonicDistanceSensor;
        }

        public void MotorLeft()
        {
            l298NMtorController.Left();
        }

        public void MotorRight()
        {
            l298NMtorController.Right();
        }

        public void MotorForward()
        {
            l298NMtorController.Forward();
        }

        public void MotorReverse()
        {
            l298NMtorController.Reverse();
        }

        public void MotorStop()
        {
            l298NMtorController.Stop();
        }

        public double GetDistanceFromUDSCSensor()
        {
            return ultraSonicDistanceSensor.GetDistanceFromSensor();
        }

        public string GetPinStatusFromMc()
        {
            return l298NMtorController.GetPinsStatus();
        }

        public string GetPinStatusFromUDSC()
        {
            return ultraSonicDistanceSensor.GetPinStatus();
        }

        public void CleanAllControllersAndSensors()
        {
            l298NMtorController.CleanUp();
            ultraSonicDistanceSensor.CleanUp();
        }
    }
}