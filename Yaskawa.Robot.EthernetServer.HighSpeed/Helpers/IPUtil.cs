using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Yaskawa.Robot.EthernetServer.HighSpeed
{
    internal static class IPUtil
    {
        public static IPAddress ToIPAddress(string hostnameOrIpAddress, AddressFamily family = AddressFamily.InterNetwork)
        {
            if (IPAddress.TryParse(hostnameOrIpAddress, out IPAddress address))
                return address;

            var addresses = Dns.GetHostAddresses(hostnameOrIpAddress);
            address = addresses.FirstOrDefault(addr => addr.AddressFamily == family);
            return address
                ?? addresses.FirstOrDefault(addr => addr.AddressFamily switch
                {
                    AddressFamily.InterNetwork => true,
                    AddressFamily.InterNetworkV6 => true,
                    _ => false,
                });
        }
    }
}
