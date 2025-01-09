using System;
using System.Linq;

namespace Yaskawa.Robot.EthernetServer.HighSpeed.Packet
{
    class PacketReq
    {
        public const byte SERVICE_GET_ATTRIBUTE_SINGLE = 0x0E;
        public const byte SERVICE_GET_ATTRIBUTE_ALL = 0x01;
        public const byte SERVICE_SET_ATTRIBUTE_SINGLE = 0x10;
        public const byte SERVICE_SET_ATTRIBUTE_ALL = 0x02;
        public const byte SERVICE_READ_PLURAL = 0x33;
        public const byte SERVICE_WRITE_PLURAL = 0x34;

        public readonly PacketHeader Header;
        readonly ushort cmd_no;
        readonly ushort inst;
        readonly byte attr;
        readonly byte service;
        readonly byte[] data;

        public PacketReq(byte division, byte req_id, ushort cmd_no, ushort inst, byte attr, byte service, byte[] data, ushort data_size)
        {
            Header = new PacketHeader(data_size, division,
                PacketHeader.HEADER_ACK_REQUEST, req_id, PacketHeader.HEADER_BLOCK_NUMBER_REQ);
            this.cmd_no = cmd_no;
            this.inst = inst;
            this.attr = attr;
            this.service = service;
            this.data = data;
        }

        public byte[] ToBytes()
        {
            var subheader = new byte[8]
            {
                0x00, 0x00, 0x00, 0x00, // | Command No.         | Instance            |
                0x00, 0x00, 0x00, 0x00, // | attribute | service | padding1 | padding2 |
            };
            BitConverterEx.WriteBytes(cmd_no, subheader, 0);
            BitConverterEx.WriteBytes(inst, subheader, 2);
            BitConverterEx.WriteBytes(attr, subheader, 4);
            BitConverterEx.WriteBytes(service, subheader, 5);

            return Header.ToBytes()
                .Concat(subheader)
                .Concat(data)
                .ToArray();
        }
    }
}
