using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RpiWebServer.Serializers {
    public class JsonSerialiser : ISerialiser {
        public T Deserialize<T>(string text) {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public string Serialize(object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }

}
