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
            return Header.ToBytes()
                .Concat(BitConverter.GetBytes(cmd_no))
                .Concat(BitConverter.GetBytes(inst))
                .Concat(new byte[] { attr, service })
                .Concat(BitConverter.GetBytes(PacketHeader.HEADER_PADDING_U16))
                .Concat(data)
                .ToArray();
        }

        public PacketReq Clone(byte[] data = null)
        {
            if (data == null)
            {
                return new PacketReq(Header.division, Header.req_id, cmd_no, inst, attr, service, this.data, (ushort)this.data.Length);
            }
            else
            {
                return new PacketReq(Header.division, Header.req_id, cmd_no, inst, attr, service, data, (ushort)data.Length);
            }
        }
    }
}
