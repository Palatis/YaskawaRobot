using System;
using System.Linq;

namespace Yaskawa.Robot.EthernetServer.HighSpeed.Packet
{
    internal class PacketAns
    {
        public readonly PacketHeader Header;
        public readonly byte service;
        public readonly byte status;
        public readonly byte added_status_size;
        public readonly ushort added_status;
        public readonly byte[] data;

        public PacketAns(byte[] _packet)
        {
            Header = new PacketHeader(_packet);
            service = _packet[24];
            status = _packet[25];
            added_status_size = _packet[26];
            added_status = BitConverter.ToUInt16(_packet, 28);
            data = _packet.Skip(PacketHeader.HEADER_SIZE).Take(Header.data_size).ToArray();
        }

        /// <summary>
        /// For debug purpose.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            var subheader = new byte[8]
            {
                0x00, 0x00, 0x00, 0x00, // | service | status | added status size | padding |
                0x00, 0x00, 0x00, 0x00, // | added status     | padding                     |
            };
            BitConverterEx.WriteBytes(service, subheader, 0);
            BitConverterEx.WriteBytes(status, subheader, 1);
            BitConverterEx.WriteBytes(added_status_size, subheader, 2);
            BitConverterEx.WriteBytes(added_status, subheader, 4);

            return Header.GetBytes()
                .Concat(subheader)
                .Concat(data)
                .ToArray();
        }
    }
}
