
namespace RpiWebServer
{
    public interface IMotorController
    {
        void Dispose();
        void Forward();
        void Left();
        void Reverse();
        void Right();
        void Stop();
    }
}