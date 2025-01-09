using System;
using System.Linq;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        public int MovePulse(int robot, int station, SpeedType spdType, int speed, MoveMode mode, Axis pos, int tool, BaseStationData bsData, out ushort err_code)
        {
            var config = new byte[16];
            BitConverterEx.WriteBytes(robot, config, 0);
            BitConverterEx.WriteBytes(station, config, 4);
            BitConverterEx.WriteBytes((uint)spdType, config, 8);
            BitConverterEx.WriteBytes(speed, config, 12);

            var data = config
                .Concat(pos.GetBytes())
                .Concat(BitConverter.GetBytes(tool))
                .Concat(bsData.GetBytes())
                .ToArray();
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x8B, (byte)mode, 1, 0x02,
                data, (ushort)data.Length);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}
