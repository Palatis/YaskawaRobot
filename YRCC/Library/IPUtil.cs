﻿
using System.Net.Sockets;
using System.Net;

namespace YRCC
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
