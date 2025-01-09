using System;
using System.Buffers;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Yaskawa.Robot.EthernetServer.HighSpeed.Packet;

/***
 * 
 * YASKAWA High Speed Ethernet Server Functions (C#)
 * 
 * Copyright (c) 2025 (Victor Tseng <palatis@gmail.com>)
 * 
 * This C# Library is implement from YASKAWA High Speed Ethernet Server, 
 * which is a UDP base robot control protocal provide by YASKAWA.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 * 
 * Authors:
 *    Victor Tseng <palatis@gmail.com>
 * 
 * Greatest Thanks To:
 *    Willy Wu <df910105@gmail.com>
 *    Ref: https://github.com/df910105/YaskawaRobot
 *    Hsinko Yu <hsinkoyu@fih-foxconn.com>
 *    Ref: https://github.com/hsinkoyu/fs100
 *    
 ***/

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    /// <summary>
    /// YASKAWA High Speed Ethernet Server Functions (C#)
    /// </summary>
    public sealed partial class MotoComHS
    {
        #region -- Field --

        Socket socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        EndPoint endPoint;

        #endregion

        #region -- Constant --

        const int TRANSMISSION_SEND = 1;
        const int TRANSMISSION_SEND_AND_RECV = 2;

        const int ERROR_SUCCESS = 0;
        const int ERROR_CONNECTION = 1;
        const int ERROR_REQUEST_ID = 2;

        /// <summary>
        /// Ex:2011/10/10 15:49
        /// </summary>
        const string DATE_PATTERN = @"yyyy/MM/dd HH\:mm";

        public const int DEFAULT_ROBOT_CONTROL_PORT = 10040;
        public const int DEFAULT_FILE_CONTROL_PORT = 10041;

        #endregion

        #region -- Property --

        /// <summary>
        /// Robot IP
        /// </summary>
        public string Remote { get; }

        /// <summary>
        /// 逾時設定(ms)
        /// </summary>
        public int TimeOut { get; }

        /// <summary>
        /// 手臂控制埠，預設10040
        /// </summary>
        public int PORT_ROBOT_CONTROL { get; }

        /// <summary>
        /// 檔案傳輸埠，預設10041
        /// </summary>
        public int PORT_FILE_CONTROL { get; }

        public Encoding MessageEncoding { get; }

        private byte requestId = 0;
        public byte RequestId => requestId;

        #endregion

        /// <summary>
        /// YASKAWA High Speed Ethernet Server
        /// </summary>
        /// <param name="remote">IP位址 ex."192.168.255.1"</param>
        /// <param name="timeout">連線逾時</param>
        public MotoComHS(string remote, Encoding encoding = default, int timeout = 800, int r_port = DEFAULT_ROBOT_CONTROL_PORT, int f_port = DEFAULT_FILE_CONTROL_PORT)
        {
            Remote = remote;
            TimeOut = timeout;
            MessageEncoding = encoding ?? Encoding.Default;
            socket.ReceiveTimeout = TimeOut;
            socket.SendTimeout = TimeOut;
            PORT_ROBOT_CONTROL = r_port;
            PORT_FILE_CONTROL = f_port;
        }

        private void Connect(int port)
        {
            if (!socket.Connected)
            {
                socket.Dispose();
                socket = new Socket(SocketType.Dgram, ProtocolType.Udp)
                {
                    ReceiveTimeout = TimeOut,
                    SendTimeout = TimeOut
                };
            }
            endPoint = new IPEndPoint(IPUtil.ToIPAddress(Remote), port);
            socket.Connect(endPoint);
        }

        private void Disconnect()
        {
            if (socket != null)
            {
                if (socket.Connected)
                    socket.Close();
                socket.Dispose();
            }
        }

        private byte[] GenerateErrorAnsPacket(byte result, ushort errno)
        {
            var bytes = new byte[32]
            {
                0x59, 0x45, 0x52, 0x43, // | Identifier ("YERC")                                    |
                0x20, 0x00, 0x00, 0x00, // | Header size           | Data size                      |
                0x33, 0x00, 0x00, 0x00, // | Reserved 1 | division | ACK               | Request ID |
                0x00, 0x00, 0x00, 0x00, // | Block No.                                              |
                0x39, 0x39, 0x39, 0x39, // | Reserved 2                                             |
                0x39, 0x39, 0x39, 0x39, // | Reserved 2                                             |
                0x00, 0x00, 0x02, 0x00, // | service    | status   | added status size | padding    |
                0x00, 0x00, 0x00, 0x00, // | added status          | padding                        |
            };
            BitConverterEx.WriteBytes(result, bytes, 25);
            BitConverterEx.WriteBytes(errno, bytes, 29);
            return bytes;
        }

        private PacketAns Transmit(PacketReq req, int port, int direction = TRANSMISSION_SEND_AND_RECV)
        {
            PacketAns ans = null;
            lock (this)
            {
                bool to_disc = !socket.Connected;
                if (!socket.Connected)
                    Connect(port);

                try
                {
                    socket.Send(req.GetBytes());
                    if (direction == TRANSMISSION_SEND_AND_RECV)
                    {
                        var ans_packet = ArrayPool<byte>.Shared.Rent(512);
                        try
                        {
                            int count = socket.Receive(ans_packet);
                            ans = new PacketAns(ans_packet);
                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(ans_packet);
                        }
                    }

                    if (req.Header.req_id != ans.Header.req_id)
                        ans = new PacketAns(GenerateErrorAnsPacket(ERROR_REQUEST_ID, 0));

                    return ans;
                }
                catch (SocketException ex)
                {
                    return new PacketAns(GenerateErrorAnsPacket(ERROR_CONNECTION, (ushort)ex.ErrorCode));
                }
                finally
                {
                    if (to_disc)
                        Disconnect();
                }
            }
        }

        private byte NextRequestId() => ++requestId;
    }
}
