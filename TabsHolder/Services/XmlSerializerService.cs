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
        public static void Serialize(string fileName,Session ses)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Session));
            StreamWriter myWriter = new StreamWriter(fileName);
            ser.Serialize(myWriter, ses);
            myWriter.Close();
        }

        public static Session Deserialize(string filePath) {
            
            var mySerializer = new XmlSerializer(typeof(Session));
            // To read the file, create a FileStream.
            var myFileStream = new FileStream(filePath, FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            var ses = (Session)mySerializer.Deserialize(myFileStream);


            return ses;
        }
    }
}
