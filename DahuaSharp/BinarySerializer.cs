using DahuaSharp.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DahuaSharp
{
    public static class BinarySerializer
    {
        private static byte[] SerializeString(String str, int maxLength)
        {
            if (maxLength <= 0) throw new ArgumentOutOfRangeException("maxLength");

            byte[] bytes = new byte[maxLength];

            if (!String.IsNullOrWhiteSpace(str))
            {
                var strBytes = Encoding.ASCII.GetBytes(str);
                Array.Copy(strBytes, bytes, Math.Min(maxLength, strBytes.Length));
            }

            return bytes;
        }

        private static String DeserializeString(byte[] bytes)
        {
            String str = null;

            if (!(bytes == null || bytes.Length == 0))
            {
                str = Encoding.ASCII.GetString(bytes);
            }

            return str;
        }

        private static byte[] SerializeInt(Int32 n)
        {
            return BitConverter.GetBytes(n).ToArray();
        }

        private static byte[] SerializeInt(Int16 n)
        {
            return BitConverter.GetBytes(n).ToArray();
        }

        internal static void Serialize(BinaryPacket bp, Stream stream)
        {
            var fieldsInfos = new List<FieldInfo>();

            fieldsInfos.AddRange(bp.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromPropertyInfo(x, bp)));
            fieldsInfos.AddRange(bp.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromFieldInfo(x, bp)));

            stream.WriteByte(bp.Header);

            foreach (var fieldInfo in fieldsInfos.OrderBy(x => x.Field.Order))
            {
                var value = fieldInfo.Value;
                if (value is Byte)
                {
                    stream.WriteByte((Byte)value);
                }
                else if (value is Byte[])
                {
                    var byteValue = (Byte[])value;
                    stream.Write(byteValue, 0, byteValue.Length);
                }
                else if (value is String)
                {
                    var byteValue = SerializeString((String)value, fieldInfo.Field.Length);
                    stream.Write(byteValue, 0, byteValue.Length);
                }
                else if (value is Int16)
                {
                    var byteValue = SerializeInt((Int16)value);
                    stream.Write(byteValue, 0, byteValue.Length);
                }
                else if (value is Int32)
                {
                    var byteValue = SerializeInt((Int32)value);
                    stream.Write(byteValue, 0, byteValue.Length);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        internal static T Deserialize<T>(Stream stream) where T : BinaryPacket, new()
        {
            T obj = new T();     
            byte header = (byte)stream.ReadByte();

            if(header != obj.Header)
            {
                throw new Exception( String.Format("Invalid packet type, expected {0}, got {1}.", obj.Header, obj.Header));
            }

            var fieldsInfos = new List<FieldInfo>();

            fieldsInfos.AddRange(typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromPropertyInfo(x)));
            fieldsInfos.AddRange(typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromFieldInfo(x)));
              

            foreach (var fieldInfo in fieldsInfos.OrderBy(x => x.Field.Order))
            {
                if (fieldInfo.MemberInfo is System.Reflection.PropertyInfo)
                {
                    var pi = (System.Reflection.PropertyInfo)fieldInfo.MemberInfo;
                    var type = pi.PropertyType;

                    if (type == typeof(Byte))
                    {                      
                        pi.SetValue(obj, (byte)stream.ReadByte());
                    }
                    else if (type == typeof(Byte[]))
                    {
                        var valueBytes = stream.ReadResponse(fieldInfo.Field.Length);  
                        pi.SetValue(obj, valueBytes);
                    }
                    else if (type == typeof(Int32))
                    {
                        var value = BitConverter.ToInt32(stream.ReadResponse(4),0);
                        pi.SetValue(obj, value);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                }
                else if (fieldInfo.MemberInfo is System.Reflection.FieldInfo)
                {

                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return obj;
        }
    }
}
