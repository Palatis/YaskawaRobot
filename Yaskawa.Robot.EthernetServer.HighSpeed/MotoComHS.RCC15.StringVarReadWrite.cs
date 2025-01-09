using System.Linq;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        /// 本頁功能確認於 2022/10/26 by Willy

        /// <summary>
        /// [RCC15] 讀取字串型(string)資料 (0x7E)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadStrData(ushort number, ref string data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7E, number, 1, 0x0E,
                new byte[0], 0);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data = MessageEncoding.GetString(ans.data.Skip(0).Take(16).ToArray());
            }
            return ans.status;
        }

        /// <summary>
        /// [RCC15] 寫入字串型(string)資料 (0x7E). 教導器可以正確顯示中文內容(Big5)，但不支援編輯
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int WriteStrData(ushort number, string data, out ushort err_code)
        {
            var bytes = MessageEncoding.GetBytes(data);
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x7E, number, 1, 0x10,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}
