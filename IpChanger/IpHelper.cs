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

        public static void SetIP(string IPAddress, string SubnetMask, string Gateway, string selectedNetworkAdapter)
        {
            var adapterConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var networkCollection = adapterConfig.GetInstances();

            foreach (ManagementObject adapter in networkCollection)
            {
                string description = adapter["Description"] as string;
                if (string.Compare(description,
                    selectedNetworkAdapter, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    try
                    {
                        var newGateway = adapter.GetMethodParameters("SetGateways");    // set def. gateway
                        newGateway["DefaultIPGateway"] = new string[] { Gateway };
                        newGateway["GatewayCostMetric"] = new int[] { 1 };

                        var newAddress = adapter.GetMethodParameters("EnableStatic");   // set IP + subnet
                        newAddress["IPAddress"] = new string[] { IPAddress };
                        newAddress["SubnetMask"] = new string[] { SubnetMask };

                        adapter.InvokeMethod("EnableStatic", newAddress, null);
                        adapter.InvokeMethod("SetGateways", newGateway, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nem sikerült beállítani a kívánt IP-címet\n {0}", ex.ToString());
                    }
                }
            }
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