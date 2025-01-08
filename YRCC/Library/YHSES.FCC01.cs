
namespace YRCC
{
    partial class YHSES
    {
        //Untest
        /*
        public int FileDelete(string fileName, out ushort err_code)
        {
            byte[] bytes = utf_8.GetBytes(fileName);
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_FILE_CONTROL, NextRequestId(),
                0x0, 0x0, 0x0, 0x09,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req.ToBytes(), PORT_FILE_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }*/
    }
}
