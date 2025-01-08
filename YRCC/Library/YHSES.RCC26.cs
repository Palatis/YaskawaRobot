using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/31 by Willy

        /// <summary>
        /// [RCC26] 讀取系統版本序號 (0x89)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="info"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadSystemInfoData(ushort number, ref SystemInfo info, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x89, number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                info.SysSoftwareVer = Encoding.ASCII.GetString(ans.data.Skip(0).Take(24).ToArray());
                info.ModelName_App = Encoding.ASCII.GetString(ans.data.Skip(24).Take(16).ToArray());
                info.ParameterVer = Encoding.ASCII.GetString(ans.data.Skip(40).Take(8).ToArray());
            }
            return ans.status;
        }
    }

    /// <summary>
    /// 系統版本資訊
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// 版本資訊
        /// </summary>
        public string SysSoftwareVer = string.Empty;

        /// <summary>
        /// 名稱/應用
        /// </summary>
        public string ModelName_App = string.Empty;

        /// <summary>
        /// 版本代號
        /// </summary>
        public string ParameterVer = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"System software version: {SysSoftwareVer},\r\n" +
                $"Model name / application: {ModelName_App},\r\n" +
                $"Parameter version: {ParameterVer}\r\n";
        }
    }
}
