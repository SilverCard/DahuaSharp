using SilverCard.DahuaSharp.Packets;
using SmallDahuaLib.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallDahuaLib
{
    public class BinarySerializer
    {
        private Stream _Stream;

        public BinarySerializer(Stream stream)
        {
            _Stream = stream;
        }

        public Task SerializeAsync(PacketBase packet, CancellationToken ct)
        {
            var bp = SerializeToBinaryPacket(packet);
            return SerializeAsync(bp, ct);
        }

        public Task SerializeAsync(PacketBase packet)
        {
            return SerializeAsync(packet, CancellationToken.None);
        }

        public BinaryPacket SerializeToBinaryPacket(PacketBase packet)
        {
            BinaryPacket bp = new BinaryPacket();
            bp.Id = packet.Id;
            Array.Copy(packet.Params, bp.Params, bp.Params.Length);
            bp.Body = packet.Body;
            SerializeHeader(packet, bp.Header);
            return bp;
        }

        public async Task SerializeAsync(BinaryPacket packet, CancellationToken ct)
        {
            byte[] packetHeader = new byte[32];
            packetHeader[0] = packet.Id;
            Array.Copy(packet.Params, 0, packetHeader, 1, packet.Params.Length);

            int len = packet.Body == null ? 0 : packet.Body.Length;
            byte[] lenBytes = BitConverter.GetBytes(len);

            Array.Copy(lenBytes, 0, packetHeader, 4, lenBytes.Length);
            Array.Copy(packet.Header, 0, packetHeader, 8, packet.Header.Length);

            await _Stream.WriteAsync(packetHeader, 0, packetHeader.Length, ct);

            if (len > 0)
            {
                await _Stream.WriteAsync(packet.Body, 0, len, ct);
            }

        }

        public Task<T> DeserializeAsync<T>() where T : PacketBase, new()
        {
            return DeserializeAsync<T>(CancellationToken.None);
        }

        public async Task<T> DeserializeAsync<T>(CancellationToken ct) where T : PacketBase, new()
        {
            var packet = await DeserializeAsync(ct);
            return DeserializeAsync<T>(packet);
        }

        public T DeserializeAsync<T>(BinaryPacket packet) where T : PacketBase, new()
        {
            T obj = new T();

            if (obj.Id != packet.Id)
            {
                throw new Exception("Wrong packet header.");
            }

            Array.Copy(packet.Params, obj.Params, obj.Params.Length);

            if (packet.Body != null)
            {
                obj.Body = packet.Body;
            }

            DeserializeHeader(obj, packet.Header);
            return obj;
        }

        public PacketBase DeserializeAsync(BinaryPacket packet)
        {
            return null;
        }

        public Task<BinaryPacket> DeserializeAsync()
        {
            return DeserializeAsync(CancellationToken.None);
        }

        public async Task<BinaryPacket> DeserializeAsync(CancellationToken ct)
        {
            byte[] packetHeader = await _Stream.ReadAllBytesAsync(32, ct);
            BinaryPacket bp = new BinaryPacket();

            bp.Id = packetHeader[0];
            Array.Copy(packetHeader, 1, bp.Params, 0, bp.Params.Length);
            int bodyLen = BitConverter.ToInt32(packetHeader, 4);
            Array.Copy(packetHeader, 8, bp.Header, 0, bp.Header.Length);

            if (bodyLen > 0)
            {
                bp.Body = await _Stream.ReadAllBytesAsync(bodyLen, ct);
            }

            return bp;
        }

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
            return BitConverter.GetBytes(n);
        }

        private static byte[] SerializeInt(Int16 n)
        {
            return BitConverter.GetBytes(n);
        }

        private static void CopyArray(byte[] source, byte[] destination, ref int destinationIdx)
        {
            Array.Copy(source, 0, destination, destinationIdx, source.Length);
            destinationIdx += source.Length;
        }

        internal static void SerializeHeader(PacketBase bp, byte[] data)
        {
            var fieldsInfos = new List<FieldInfo>();

            fieldsInfos.AddRange(bp.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromPropertyInfo(x, bp)));
            fieldsInfos.AddRange(bp.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromFieldInfo(x, bp)));

            int idx = 0;

            foreach (var fieldInfo in fieldsInfos.OrderBy(x => x.Field.Order))
            {
                var value = fieldInfo.Value;
                if (value is Byte)
                {
                    data[idx] = 0;
                    idx++;
                }
                else if (value is Byte[])
                {
                    CopyArray((Byte[])value, data, ref idx);
                }
                else if (value is String)
                {
                    CopyArray(SerializeString((String)value, fieldInfo.Field.Length), data, ref idx);
                }
                else if (value is Int16)
                {
                    CopyArray(BitConverter.GetBytes((Int16)value), data, ref idx);
                }
                else if (value is Int32)
                {
                    CopyArray(BitConverter.GetBytes((Int32)value), data, ref idx);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        internal static void DeserializeHeader(PacketBase packet, byte[] header)
        {
            var type = packet.GetType();
            var fieldsInfos = new List<FieldInfo>();
            fieldsInfos.AddRange(type.GetProperties().Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromPropertyInfo(x)));
            fieldsInfos.AddRange(type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(p => Attribute.IsDefined(p, typeof(FieldAttribute))).Select(x => FieldInfo.FromFieldInfo(x)));

            int idx = 0;

            foreach (var fieldInfo in fieldsInfos.OrderBy(x => x.Field.Order))
            {
                if (fieldInfo.MemberInfo is System.Reflection.PropertyInfo)
                {
                    var pi = (System.Reflection.PropertyInfo)fieldInfo.MemberInfo;
                    var propType = pi.PropertyType;

                    if (propType == typeof(Byte))
                    {
                        pi.SetValue(packet, header[idx]);
                        idx++;
                    }
                    else if (propType == typeof(Byte[]))
                    {
                        var valueBytes = new byte[fieldInfo.Field.Length];
                        Array.Copy(header, idx, valueBytes, 0, valueBytes.Length);
                        idx += valueBytes.Length;
                        pi.SetValue(packet, valueBytes);
                    }
                    else if (propType == typeof(Int32))
                    {
                        var value = BitConverter.ToInt32(header, idx);
                        idx += 4;
                        pi.SetValue(packet, value);
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

        }
    }
}

