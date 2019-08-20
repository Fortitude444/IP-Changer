using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            getCurrentInformations();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getCurrentInformations();
        }

        public void getCurrentInformations()
        {
            currentIpAddress.Text = GetLocalIPAddress();
            currentSubnetMask.Text = IPClassTester.GetSubnetMask(currentIpAddress.Text);
            currentDefaultGateway.Text = GetDefaultGateway().ToString();
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

        class IPClassTester

        {
            static public string GetSubnetMask(string ipaddress)
            {
                uint firstOctet = ReturnFirtsOctet(ipaddress);
                if (firstOctet >= 0 && firstOctet <= 127)
                    return "255.0.0.0";

                else if (firstOctet >= 128 && firstOctet <= 191)
                    return "255.255.0.0";

                else if (firstOctet >= 192 && firstOctet <= 223)
                    return "255.255.255.0";

                else return "0.0.0.0";
            }

            static public uint ReturnFirtsOctet(string ipAddress)

            {
                IPAddress iPAddress = System.Net.IPAddress.Parse(ipAddress);
                byte[] byteIP = iPAddress.GetAddressBytes();
                uint ipInUint = (uint)byteIP[0];

                return ipInUint;
            }
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
    }
}
