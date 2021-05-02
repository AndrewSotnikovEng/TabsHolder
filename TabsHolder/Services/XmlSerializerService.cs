using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TabsHolder.Data;

namespace TabsHolder.Services
{
    class XmlSerializerService
    {
        public static void SerializeSeesion(string fileName,Session ses)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Session));
            StreamWriter myWriter = new StreamWriter(fileName);
            ser.Serialize(myWriter, ses);
            myWriter.Close();
        }

        public static Session DeserializeSession(string filePath) {
            
            var mySerializer = new XmlSerializer(typeof(Session));
            // To read the file, create a FileStream.
            var myFileStream = new FileStream(filePath, FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            var ses = (Session)mySerializer.Deserialize(myFileStream);
            myFileStream.Close();

            return ses;
        }

        public static void SerializeConfg(string fileName, Config cfg)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Config));
            StreamWriter myWriter = new StreamWriter(fileName);
            ser.Serialize(myWriter, cfg);
            myWriter.Close();
        }

        public static Config DeserializeConfig(string filePath)
        {

            var mySerializer = new XmlSerializer(typeof(Config));
            // To read the file, create a FileStream.
            var myFileStream = new FileStream(filePath, FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            var cfg = (Config)mySerializer.Deserialize(myFileStream);
            myFileStream.Close();

            return cfg;
        }

    }
}
