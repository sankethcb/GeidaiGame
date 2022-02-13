using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
{
    public class IOUtility
    {
        [System.ObsoleteAttribute("TODO: Remove Binary Formatter")]
        public static void SaveFile(object file, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, file);
            }
        }

        [System.ObsoleteAttribute("TODO: Remove Binary Formatter")]
        public static T LoadFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            using (var stream = File.Open(filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (T)(formatter.Deserialize(stream));
            }
        }

        public static void SaveFile(byte[] file, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                stream.Write(file, 0, file.Length);
            }
        }

        public static byte[] LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            long fileLength = new FileInfo(filePath).Length;

            if (fileLength == 0)
                return null;

            byte[] file = null;

            using (var stream = File.Open(filePath, FileMode.Create))
            {
                stream.Read(file, 0, (int)fileLength);
            }

            return file;
        }
    }

}
