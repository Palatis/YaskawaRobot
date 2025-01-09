﻿using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        /// 本頁功能確認於 2022/10/26 by Willy

        /// <summary>
        /// [RCC11] 讀取位元組資料 (0x7A)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadByteData(ushort number, ref byte data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7A, number, 1, 0x0E,
                new byte[0], 0);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data = ans.data[0];
            }
            return ans.status;
        }

        /// <summary>
        /// [RCC11] 寫入位元組資料 (0x7A)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int WriteByteData(ushort number, byte data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7A, number, 1, 0x10,
                new byte[1] { data }, 1);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}