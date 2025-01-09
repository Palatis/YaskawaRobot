using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yaskawa.Robot.EthernetServer.HighSpeed.Packet
{
    internal class PacketHeader
    {
        public const ushort HEADER_SIZE = 0x20;

        public const byte HEADER_DIVISION_ROBOT_CONTROL = 1;
        public const byte HEADER_DIVISION_FILE_CONTROL = 2;

        public const byte HEADER_ACK_REQUEST = 0;
        public const byte HEADER_ACK_NOT_REQUEST = 1;

        public const uint HEADER_BLOCK_NUMBER_REQ = 0;

        public readonly ushort data_size;
        public readonly byte division;
        public readonly byte ack;
        public readonly byte req_id;
        public readonly uint block_no;

        public PacketHeader(ushort _data_size, byte _division, byte _ack, byte _req_id, uint _block_no)
        {
            data_size = _data_size;
            division = _division;
            ack = _ack;
            req_id = _req_id;
            block_no = _block_no;
        }

        public PacketHeader(byte[] packet)
        {
            data_size = BitConverter.ToUInt16(packet, 6);
            division = packet[9];
            ack = packet[10];
            req_id = packet[11];
            block_no = BitConverter.ToUInt32(packet, 12);
        }

        public byte[] ToBytes()
        {
            var header = new byte[24]
{
                0x59, 0x45, 0x52, 0x43, // | Identifier ("YERC")                      |
                0x00, 0x00, 0x00, 0x00, // | Header size           | Data size        |
                0x33, 0x00, 0x00, 0x00, // | Reserved 1 | division | ACK | Request ID |
                0x00, 0x00, 0x00, 0x00, // | Block No.                                |
                0x39, 0x39, 0x39, 0x39, // | Reserved 2                               |
                0x39, 0x39, 0x39, 0x39, // | Reserved 2                               |
            };
            BitConverterEx.WriteBytes(HEADER_SIZE, header, 4);
            BitConverterEx.WriteBytes(data_size, header, 6);
            BitConverterEx.WriteBytes(division, header, 9);
            BitConverterEx.WriteBytes(ack, header, 10);
            BitConverterEx.WriteBytes(req_id, header, 11);
            BitConverterEx.WriteBytes(block_no, header, 12);
            return header;
        }
    }
}
