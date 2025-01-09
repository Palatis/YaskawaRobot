using System;
using System.Linq;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        public int MoveCartesian(int robot, int station, SpeedType spdType, int speed, MoveMode mode, CoordinateType coord, CartesianPositionData posData, BaseStationData bsData, out ushort err_code)
        {
            var config = new byte[20];
            BitConverterEx.WriteBytes(robot, config, 0);
            BitConverterEx.WriteBytes(station, config, 4);
            BitConverterEx.WriteBytes((uint)spdType, config, 8);
            BitConverterEx.WriteBytes(speed, config, 12);
            BitConverterEx.WriteBytes((uint)coord, config, 16);

            var data = config
                .Concat(posData.GetBytes())
                .Concat(bsData.GetBytes())
                .ToArray();
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x8A, (byte)mode, 1, 0x02,
                data, (ushort)data.Length);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }

    public class CartesianPositionData
    {
        public int X;
        public int Y;
        public int Z;
        public int Tx;
        public int Ty;
        public int Tz;
        public int Type;
        public int ExpandedType;
        public int ToolNumber;
        public int UserCoordinate;

        public override string ToString()
        {
            return $"Type: {Type}\r\n" +
                $"ExpandedType: {ExpandedType}\r\n" +
                $"Position:\r\n" +
                $"X: {X / 1000.0:0.000}, " +
                $"Y: {Y / 1000.0:0.000}, " +
                $"X: {Z / 1000.0:0.000}, " +
                $"Tx: {Tx / 10000.0:0.0000}, " +
                $"Ty: {Ty / 10000.0:0.0000}, " +
                $"Tz: {Tz / 10000.0:0.0000}\r\n" +
                $"ToolNumber: {ToolNumber}\r\n" +
                $"UserCoordinate: {UserCoordinate}\r\n";
        }

        public byte[] GetBytes()
        {
            var pos = new byte[48];
            BitConverterEx.WriteBytes(X, pos, 0);
            BitConverterEx.WriteBytes(Y, pos, 4);
            BitConverterEx.WriteBytes(Z, pos, 8);
            BitConverterEx.WriteBytes(Tx, pos, 12);
            BitConverterEx.WriteBytes(Ty, pos, 16);
            BitConverterEx.WriteBytes(Tz, pos, 20);
            BitConverterEx.WriteBytes(Type, pos, 32);
            BitConverterEx.WriteBytes(ExpandedType, pos, 36);
            BitConverterEx.WriteBytes(ToolNumber, pos, 40);
            BitConverterEx.WriteBytes(UserCoordinate, pos, 44);
            return pos;
        }
    }

    public enum SpeedType : uint
    {
        /// <summary>% (Link operation)</summary>
        Percent = 0,
        /// <summary>V (Cartesian operation)</summary>
        Velocity = 1,
        /// <summary>VR (Cartesian operation)</summary>
        RotationalVelocity = 2,
    }

    public enum CoordinateType : uint
    {
        BaseCoordinate = 16,
        RobotCoordinate = 17,
        UserCoordinate = 18,
        ToolCoordinate = 19,
    }

    public enum MoveMode : uint
    {
        LinkAbsolute = 1,
        StraightAbsolute = 2,
        StraightIncrement = 3,
    }
}
