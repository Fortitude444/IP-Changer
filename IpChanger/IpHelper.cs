using System;
using System.Net;
using System.Net.Sockets;

namespace IpChanger
{
    public static class IpHelper
    {
        public static string GetSubnetMask(string ipaddress)
        {
            uint firstOctet = ReturnFirstOctet(ipaddress);
            if (firstOctet >= 0 && firstOctet <= 127 && firstOctet != 0)
                return "255.0.0.0";

            else if (firstOctet >= 128 && firstOctet <= 191)
                return "255.255.0.0";

            else if (firstOctet >= 192 && firstOctet <= 223)
                return "255.255.255.0";

            else if (ipaddress.Contains(".") == false)
                return "";

            else return "0.0.0.0";
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GatewayAutoComplete(string ipaddress)
        {
            if (ipaddress.Contains(".") == true)
            {
                string gateWayAddress = ipaddress.Substring(0, ipaddress.LastIndexOf("."));
                return gateWayAddress + ".1";
            }
            else
                return "";
        }

        private static uint ReturnFirstOctet(string ipAddress)
        {
            IPAddress IP;
            bool flag = IPAddress.TryParse(ipAddress, out IP);
            if (flag == true)
            {
                IP = IPAddress.Parse(ipAddress);
                byte[] byteIP = IP.GetAddressBytes();
                uint ipInUint = (uint)byteIP[0];

                return ipInUint;
            }
            else
                return 0;
        }
    }
}