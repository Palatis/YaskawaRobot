using System;
using System.Text;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    partial class MotoComHS
    {
        /// 本頁功能確認於 2022/10/27 by Willy

        /// <summary>
        /// [RCC01] 讀取目前異常資訊 (0x70)
        /// </summary>
        /// <param name="last_number">Range: 1st - 4th.</param>
        /// <param name="alarm"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadAlarmData(ushort last_number, ref AlarmData alarm, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, NextRequestId(),
                0x70, last_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req, PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                AlarmDataDecode(alarm, ans.data);
            }
            return ans.status;
        }

        private void AlarmDataDecode(AlarmData alarm, byte[] packetData)
        {
            alarm.Code = BitConverter.ToUInt32(packetData, 0);
            alarm.Data = BitConverter.ToUInt32(packetData, 4);
            alarm.Type = (AlarmType)BitConverter.ToUInt32(packetData, 8);
            var timeString = Encoding.ASCII.GetString(packetData, 12, 16).TrimEnd('\0');
            if (DateTime.TryParseExact(timeString, DATE_PATTERN, null,
                System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                alarm.Time = dateTime;
            }
            alarm.Name = MessageEncoding.GetString(packetData, 24, 32).TrimEnd('\0');
        }
    }

    /// <summary>
    /// 異常資訊
    /// </summary>
    public class AlarmData
    {
        /// <summary>
        /// 發生時間
        /// </summary>
        public DateTime Time = new DateTime();

        /// <summary>
        /// 描述
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// 代號
        /// </summary>
        public uint Code = 0;

        /// <summary>
        /// 次代號(sub code)
        /// </summary>
        public uint Data = 0;

        /// <summary>
        /// 異常分類
        /// </summary>
        public AlarmType Type = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Time: {Time:g}\r\n" +
                $"Name: {Name}\r\n" +
                $"Code: {Code}, " +
                $"Data: {Data}, " +
                $"Type: {Type}";
        }
    }

    public enum AlarmType : uint
    {
        /// <summary>No alarm</summary>
        NoAlarm = 0,
        /// <summary>Decimal UNSIGNED SHORT type (ex: [1])</summary>
        DecimalUShort = 1,
        /// <summary>UNSIGNED CHAR bit pattern (ex: [0000_0001])</summary>
        ByteBitPattern = 2,
        /// <summary>User axis type (ex: [SLURBT])</summary>
        UserAxis = 3,
        /// <summary>Spacial coordinate type (ex: [XYZ])</summary>
        SpacialCoordinate = 4,
        /// <summary>Robot coordinate type (ex: {XYZRxRyRz])</summary>
        RobotCoordinate = 5,
        /// <summary>Conveyor characteristic file (ex: [123])</summary>
        ConveyorCharacteristicFile = 6,
        /// <summary>Control group type for robot & station (ex: [R1R2S1S2])</summary>
        RobotStationControlGroup = 8,
        /// <summary>Decimal SHORT type (ex: [-1])</summary>
        DecimalShort = 9,
        /// <summary>UNSIGNED SHORT bit pattern (ex: [0000_0000_0000_0001])</summary>
        UShortBitPattern = 10,
        /// <summary>Control group type for robot only (ex: [R1])</summary>
        RobotControlGroup = 11,
        /// <summary>Control gorup type for robot, station, and base (ex: [R1S1B1])</summary>
        AllControlGroup = 12,
        /// <summary>Control group LOW/HIGH logical axis (ex: [R1: LOW SLURBT, HIGH SLURBT])</summary>
        ControlGroupLowHighLogicalAxis = 20,
        /// <summary>Control group MIN/MAX logical axis (ex: [R1: MIN SLURBT, MAX SLURBT])</summary>
        ControlGroupMinMaxLogicalAxis = 21,
        /// <summary>Control gorup MIN/MAX spacial coordinate (ex: [R1: MIN XYZ, MAX XYZ])</summary>
        ControlGroupMinMaxSpacialCoordinate = 22,
        /// <summary>Logical axis of both control group 1 and control group 2 (ex: [R1: SLURBT, R2: SLURBT])</summary>
        LogicalAxisEachCGroup = 23,
        /// <summary>Logical axis 1 and 2 of the control group (ex: [R1: SLURBT, SLURBT])</summary>
        LogicalAxisCGroups = 24,
        /// <summary>Logical axis of the control group and UNSIGNED CHAR type (ex: [R1: SLURBT 1])</summary>
        LogicalAxisCGroupByte = 25,
        /// <summary>Control group and UNSIGNED CHAR type (ex: [R1: 1])</summary>
        LogicalAxisByte = 26,
    }
}
