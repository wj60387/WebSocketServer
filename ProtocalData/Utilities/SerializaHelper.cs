using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProtocalData.Utilities
{
    public static class SerializaHelper
    {
        public static byte[] Serialize(object obj)
        {

            IFormatter formattrt = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formattrt.Serialize(stream, obj);
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            return buffer;

        }
        public static T DeSerialize<T>(byte[] bytes)  
        {
            T t = default(T);
            IFormatter formattrt = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];
            t = (T)formattrt.Deserialize(stream);
            stream.Flush();
            stream.Close();
            return t;
        }
    }
}
