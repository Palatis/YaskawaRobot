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
            var data = new byte[36];
            BitConverterEx.WriteBytes(Base1, data, 0);
            BitConverterEx.WriteBytes(Base2, data, 4);
            BitConverterEx.WriteBytes(Base3, data, 8);
            BitConverterEx.WriteBytes(Station1, data, 12);
            BitConverterEx.WriteBytes(Station2, data, 16);
            BitConverterEx.WriteBytes(Station3, data, 20);
            BitConverterEx.WriteBytes(Station4, data, 24);
            BitConverterEx.WriteBytes(Station5, data, 28);
            BitConverterEx.WriteBytes(Station6, data, 32);
            return data;
        }
    }
}
