﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YHSES.Packet;

namespace YHSES.Library
{
    partial class YaskawaLib
    {
        public int ReadPositionData(ushort number, ref Posistion config, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7F, number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                config.DataType = BitConverter.ToUInt32(ans.data, 0);
                config.Figure = BitConverter.ToUInt32(ans.data, 4);
                config.ToolNumber = BitConverter.ToUInt32(ans.data, 8);
                config.UserCoordNumber = BitConverter.ToUInt32(ans.data, 12);
                config.ExtendedType = BitConverter.ToUInt32(ans.data, 16);
                config.AxisData.Axis_1 = BitConverter.ToInt32(ans.data, 20);
                config.AxisData.Axis_2 = BitConverter.ToInt32(ans.data, 24);
                config.AxisData.Axis_3 = BitConverter.ToInt32(ans.data, 28);
                config.AxisData.Axis_4 = BitConverter.ToInt32(ans.data, 32);
                config.AxisData.Axis_5 = BitConverter.ToInt32(ans.data, 36);
                config.AxisData.Axis_6 = BitConverter.ToInt32(ans.data, 40);
                config.AxisData.Axis_7 = BitConverter.ToInt32(ans.data, 44);
                config.AxisData.Axis_8 = BitConverter.ToInt32(ans.data, 48);
            }
            return ans.status;
        }

        public int WritePositionData(ushort number, Posistion config, out ushort err_code)
        {
            var bytes = ParsePositionDataBytes(config);
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7F, number, 0, 0x02,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                config.DataType = BitConverter.ToUInt32(ans.data, 0);
                config.Figure = BitConverter.ToUInt32(ans.data, 4);
                config.ToolNumber = BitConverter.ToUInt32(ans.data, 8);
                config.UserCoordNumber = BitConverter.ToUInt32(ans.data, 12);
                config.ExtendedType = BitConverter.ToUInt32(ans.data, 16);
                config.AxisData.Axis_1 = BitConverter.ToInt32(ans.data, 20);
                config.AxisData.Axis_2 = BitConverter.ToInt32(ans.data, 24);
                config.AxisData.Axis_3 = BitConverter.ToInt32(ans.data, 28);
                config.AxisData.Axis_4 = BitConverter.ToInt32(ans.data, 32);
                config.AxisData.Axis_5 = BitConverter.ToInt32(ans.data, 36);
                config.AxisData.Axis_6 = BitConverter.ToInt32(ans.data, 40);
                config.AxisData.Axis_7 = BitConverter.ToInt32(ans.data, 44);
                config.AxisData.Axis_8 = BitConverter.ToInt32(ans.data, 48);
            }
            return ans.status;
        }

        private byte[] ParsePositionDataBytes(Posistion config)
        {
            IEnumerable<byte> p = BitConverter.GetBytes(config.DataType);
            p.Concat(BitConverter.GetBytes(config.Figure));
            p.Concat(BitConverter.GetBytes(config.ToolNumber));
            p.Concat(BitConverter.GetBytes(config.UserCoordNumber));
            p.Concat(BitConverter.GetBytes(config.ExtendedType));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_1));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_2));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_3));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_4));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_5));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_6));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_7));
            p.Concat(BitConverter.GetBytes(config.AxisData.Axis_8));

            return p.ToArray();
        }
    }
}
