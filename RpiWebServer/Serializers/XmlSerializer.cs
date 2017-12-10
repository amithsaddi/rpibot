using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RpiWebServer.Serializers {
    public class XmlSerialiser : ISerialiser {
        private Stream GetStream(string str) {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            var stream = new MemoryStream(byteArray);
            return stream;
        }

        public T Deserialize<T>(string text) {
            var stream = GetStream(text);
            var serializer = new XmlSerializer(typeof(T));
            T obj = (T)serializer.Deserialize(stream);
            return obj;
        }

        public string Serialize(object obj) {
            var xmlSerializer = new XmlSerializer(obj.GetType());
            using (StringWriter textWriter = new StringWriter()) {
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }
    }
}
