using RpiWebServer.Helpers;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace RpiWebServer
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();

        public WebServer(string prefixes)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");            
            
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
                                HttpHelper.RequestProcess(ctx);

                                //Response
                                string rstr = HttpHelper.ResultString;
                                ctx = ResponseProcess(ctx, rstr);                                
                            }
                            catch { } // suppress any exceptions
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
