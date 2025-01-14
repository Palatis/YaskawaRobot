using System.Linq;
using System.Text;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        /// 本頁功能確認於 2022/11/14 by Willy

        /// <summary>
        /// [RCC05] 讀取各軸名稱 (0x74)
        /// </summary>
        /// <param name="robot_number"></param>
        /// <param name="config"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadAxisName(ushort robot_number, ref AxisName config, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x74, robot_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                config.Axis_1 = Encoding.UTF8.GetString(ans.data, 0, 4).Trim();
                config.Axis_2 = Encoding.UTF8.GetString(ans.data, 4, 4).Trim();
                config.Axis_3 = Encoding.UTF8.GetString(ans.data, 8, 4).Trim();
                config.Axis_4 = Encoding.UTF8.GetString(ans.data, 12, 4).Trim();
                config.Axis_5 = Encoding.UTF8.GetString(ans.data, 16, 4).Trim();
                config.Axis_6 = Encoding.UTF8.GetString(ans.data, 20, 4).Trim();
                config.Axis_7 = Encoding.UTF8.GetString(ans.data, 24, 4).Trim();
                config.Axis_8 = Encoding.UTF8.GetString(ans.data, 28, 4).Trim();
            }
            return ans.status;
        }
    }

    /// <summary>
    /// 軸名稱
    /// </summary>
    public class AxisName
    {
        /// <summary>
        /// 
        /// </summary>
        public string Axis_1;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_2;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_3;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_4;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_5;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_6;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_7;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_8;
    }
}
