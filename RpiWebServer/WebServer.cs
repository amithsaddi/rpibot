using RpiWebServer.Controllers;
using RpiWebServer.Helpers;
using RpiWebServer.Sensors;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace RpiWebServer
{
    public class WebServer : PiCarHelper
    {
        private readonly HttpListener _listener = new HttpListener();

        public WebServer(string prefixes, IL298NMotorController iL298NMotorController, IUltraSonicDistanceSensor ultraSonicDistanceSensor) : base(iL298NMotorController, ultraSonicDistanceSensor)
        {
            
            _listener.Prefixes.Add(prefixes);
            Start();
        }        

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                //Request
                                RequestProcess(ctx);

                                //Response
                                string rstr = ResultString;
                                ctx = ResponseProcess(ctx, rstr);                                
                            }
                            catch(Exception ex)
                            {
                            } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();

            CleanUp();
        }

        public void Start()
        {
            _listener.Start();
        }

        private HttpListenerContext ResponseProcess(HttpListenerContext ctx, string rstr)
        {
            byte[] buf = Encoding.UTF8.GetBytes(rstr);
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            return ctx;
        }
    }
}
