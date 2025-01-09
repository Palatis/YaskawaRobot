using System;
using System.Linq;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    public class BaseStationData
    {
        public int Base1;
        public int Base2;
        public int Base3;
        public int Station1;
        public int Station2;
        public int Station3;
        public int Station4;
        public int Station5;
        public int Station6;

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(Base1)
                .Concat(BitConverter.GetBytes(Base2))
                .Concat(BitConverter.GetBytes(Base3))
                .Concat(BitConverter.GetBytes(Station1))
                .Concat(BitConverter.GetBytes(Station2))
                .Concat(BitConverter.GetBytes(Station3))
                .Concat(BitConverter.GetBytes(Station4))
                .Concat(BitConverter.GetBytes(Station5))
                .Concat(BitConverter.GetBytes(Station6))
                .ToArray();
        }
    }
}
