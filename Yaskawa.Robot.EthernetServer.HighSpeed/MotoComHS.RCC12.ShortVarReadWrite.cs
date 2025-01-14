﻿using System;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        /// 本頁功能確認於 2022/10/26 by Willy

        /// <summary>
        /// [RCC12] 讀取整數型(short)資料 (0x7B)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadIntData(ushort number, ref short data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7B, number, 1, 0x0E,
                new byte[0], 0);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data = BitConverter.ToInt16(ans.data, 0);
            }
            return ans.status;
        }

        /// <summary>
        /// [RCC12] 寫入整數型(short)資料 (0x7B)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int WriteIntData(ushort number, short data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7B, number, 1, 0x10,
                BitConverter.GetBytes(data), 2);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}
