namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    internal static class BitConverterEx
    {
        /// <summary>
        /// Writes an unsigned 32-bit integer (uint) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(uint n, byte[] buffer, int offset = 0)
        {
            unchecked
            {
                buffer[offset] = (byte)(n & 0xFF);
                buffer[offset + 1] = (byte)((n >> 8) & 0xFF);
                buffer[offset + 2] = (byte)((n >> 16) & 0xFF);
                buffer[offset + 3] = (byte)((n >> 24) & 0xFF);
            }
            return offset + 4;
        }

        /// <summary>
        /// Writes a signed 32-bit integer (int) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(int n, byte[] buffer, int offset = 0) =>
            WriteBytes(unchecked((uint)n), buffer, offset);

        /// <summary>
        /// Writes an unsigned 16-bit integer (ushort) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(ushort n, byte[] buffer, int offset = 0)
        {
            unchecked
            {
                buffer[offset] = (byte)(n & 0xFF);
                buffer[offset + 1] = (byte)((n >> 8) & 0xFF);
            }
            return offset + 2;
        }

        /// <summary>
        /// Writes a signed 16-bit integer (short) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(short n, byte[] buffer, int offset = 0) =>
            WriteBytes(unchecked((ushort)n), buffer, offset);

        /// <summary>
        /// Writes an unsigned 64-bit integer (ulong) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(ulong n, byte[] buffer, int offset = 0)
        {
            unchecked
            {
                buffer[offset] = (byte)(n & 0xFF);
                buffer[offset + 1] = (byte)((n >> 8) & 0xFF);
                buffer[offset + 2] = (byte)((n >> 16) & 0xFF);
                buffer[offset + 3] = (byte)((n >> 24) & 0xFF);
                buffer[offset + 4] = (byte)((n >> 32) & 0xFF);
                buffer[offset + 5] = (byte)((n >> 40) & 0xFF);
                buffer[offset + 6] = (byte)((n >> 48) & 0xFF);
                buffer[offset + 7] = (byte)((n >> 56) & 0xFF);
            }
            return offset + 8;
        }

        /// <summary>
        /// Writes a signed 64-bit integer (long) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(long n, byte[] buffer, int offset = 0) =>
            WriteBytes(unchecked((ulong)n), buffer, offset);

        /// <summary>
        /// Writes an unsigned 8-bit integer (byte) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(byte n, byte[] buffer, int offset = 0)
        {
            buffer[offset] = n;
            return offset + 1;
        }

        /// <summary>
        /// Writes a signed 8-bit integer (sbyte) to a byte array starting at the specified offset.
        /// </summary>
        public static int WriteBytes(sbyte n, byte[] buffer, int offset = 0)
        {
            buffer[offset] = unchecked((byte)n);
            return offset + 1;
        }
    }
}
