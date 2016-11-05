using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DF.Finance.Common
{
    public class XmlHelper
    {
        public static void SerializeInternal(Stream stream, object o, Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "";
            // 不生成声明头
            settings.OmitXmlDeclaration = true;
            // 强制指定命名空间，覆盖默认的命名空间。
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o, namespaces);
                writer.Close();
            }
        }
        public static string Serialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SerializeInternal(stream, o, encoding);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public static T Deserialize<T>(string s, Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }
    }
}
