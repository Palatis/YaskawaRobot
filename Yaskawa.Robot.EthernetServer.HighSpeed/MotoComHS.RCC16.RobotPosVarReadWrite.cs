using System;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        /// 本頁功能確認於 2022/10/26 by Willy

        /// <summary>
        /// [RCC16] 讀取機器人位置資料 (0x7F)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="config"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadPosData(ushort number, ref Position config, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7F, number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
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

        /// <summary>
        /// [RCC16] 寫入機器人位置資料 (0x7F)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="config"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int WritePosData(ushort number, Position config, out ushort err_code)
        {
            var bytes = config.GetBytes();
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7F, number, 0, 0x02,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}
