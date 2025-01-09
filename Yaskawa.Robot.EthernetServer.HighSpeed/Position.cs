using System;
using System.Linq;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    /// <summary>
    /// 機器人位置原始資訊，請留意型態(pulse/coord)以及轉換單位。
    /// </summary>
    public class Position
    {
        /// <summary>
        /// 資料型態(pulse/coord)
        /// </summary>
        public uint DataType = 0;

        /// <summary>
        /// 手臂關節姿態。Refer "ch.3.9.4.12 Flip/No flip" in Operator's Manual.
        /// </summary>
        public uint Figure = 0;

        /// <summary>
        /// 工具編號
        /// </summary>
        public uint ToolNumber = 0;

        /// <summary>
        /// 使用者座標編號
        /// </summary>
        public uint UserCoordNumber = 0;

        /// <summary>
        /// Extended type. Refer "ch.3.9.4.12 Flip/No flip" in Operator's Manual.
        /// </summary>
        public uint ExtendedType = 0;

        /// <summary>
        /// 
        /// </summary>
        public readonly Axis AxisData = new Axis();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (DataType == 0)
            {
                return $"DataType: {DataType} (Pulse),\r\n" +
                  $"Figure: {Figure},\r\n" +
                  $"ToolNumber: {ToolNumber},\r\n" +
                  $"UserCoordNumber: {UserCoordNumber},\r\n" +
                  $"ExtendedType: {ExtendedType},\r\n" +
                  $"AxisData:\r\n{AxisData}\r\n";
            }
            else if (DataType == 16)
            {
                return $"DataType: {DataType} (Coordinate),\r\n" +
                  $"Figure: {Figure},\r\n" +
                  $"ToolNumber: {ToolNumber},\r\n" +
                  $"UserCoordNumber: {UserCoordNumber},\r\n" +
                  $"ExtendedType: {ExtendedType},\r\n" +
                  $"AxisData:\r\n" +
                  $"X: {AxisData.Axis_1 / 1000.0:0.000}, " +
                  $"Y: {AxisData.Axis_2 / 1000.0:0.000}, " +
                  $"Z: {AxisData.Axis_3 / 1000.0:0.000}, " +
                  $"Rx: {AxisData.Axis_4 / 10000.0:0.0000}, " +
                  $"Ry: {AxisData.Axis_5 / 10000.0:0.0000}, " +
                  $"Rz: {AxisData.Axis_6 / 10000.0:0.0000}, " +
                  $"ax7: {AxisData.Axis_7}, " +
                  $"ax8: {AxisData.Axis_8}\r\n";
            }
            else
            {
                return "Undefined data type";
            }
        }

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(DataType)
                .Concat(BitConverter.GetBytes(Figure))
                .Concat(BitConverter.GetBytes(ToolNumber))
                .Concat(BitConverter.GetBytes(UserCoordNumber))
                .Concat(BitConverter.GetBytes(ExtendedType))
                .Concat(AxisData.GetBytes())
                .ToArray();
        }
    }

    /// <summary>
    /// 軸資訊，請留意型態(pulse/coord)以及轉換單位。
    /// </summary>
    public class Axis
    {
        /// <summary>
        /// 
        /// </summary>
        public int Axis_1 = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Axis_2 = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Axis_3 = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Axis_4 = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Axis_5 = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Axis_6 = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Axis_7 = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Axis_8 = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"1: {Axis_1}, " +
                $"2: {Axis_2}, " +
                $"3: {Axis_3}, " +
                $"4: {Axis_4}, " +
                $"5: {Axis_5}, " +
                $"6: {Axis_6}, " +
                $"7: {Axis_7}, " +
                $"8: {Axis_8}";
        }

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(Axis_1)
                .Concat(BitConverter.GetBytes(Axis_2))
                .Concat(BitConverter.GetBytes(Axis_3))
                .Concat(BitConverter.GetBytes(Axis_4))
                .Concat(BitConverter.GetBytes(Axis_5))
                .Concat(BitConverter.GetBytes(Axis_6))
                .Concat(BitConverter.GetBytes(Axis_7))
                .Concat(BitConverter.GetBytes(Axis_8))
                .ToArray();
        }
    }
}
