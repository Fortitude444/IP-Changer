using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public static string SubnetAutoComplete(string IPTextbox)
        {
            return IpHelper.GetSubnetMask(IPTextbox);
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
        public static IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .FirstOrDefault();
        }

        public static void SetIP(string ipAddress, string subnetMask, string defaultGateway, string networkAdapterName)
        {
            const string commandTemplate =
                "interface ipv4 set address name=\"{0}\" source=static address={1} mask={2} gateway={3}";

            string arguments = string.Format(commandTemplate, networkAdapterName, ipAddress, subnetMask, defaultGateway);

            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "netsh";
            startInfo.Arguments = arguments;

            Process cmdProcess = new Process();
            cmdProcess.StartInfo = startInfo;
            cmdProcess.Start();
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