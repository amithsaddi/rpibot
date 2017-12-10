using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RpiWebServer.Serializers {
    public interface ISerialiser
    {
        string Serialize(object obj);
        T Deserialize<T>(string text);
    }
}
