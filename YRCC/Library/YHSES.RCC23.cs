﻿using System;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/31 by Willy

        /// <summary>
        /// [RCC23] 程式選擇 (0x86)
        /// </summary>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int StartJob(out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x86, 1, 0x01, 0x10,
                BitConverter.GetBytes(1), 4);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}
