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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadUserConfig();
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

        public void setIP(string IPAddress, string SubnetMask, string Gateway)
        {
            /*
             * run as admin!
             * hiba: nem a megfelelő ip-t állítja be
             * 
            string arguments = string.Format("netsh interface ipv4 set address name=\"Ethernet\" static {0} {1} {2}",IPAddress,SubnetMask,Gateway);
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", arguments);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            Process.Start(procStartInfo);
            */
        }

        class IPClassTester

        {
            static public string GetSubnetMask(string ipaddress)
            {
                uint firstOctet = ReturnFirtsOctet(ipaddress);
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

            static public string gatewayAutoComplete(string ipaddress)
            {
                if (ipaddress.Contains(".") == true)
                {
                    string gateWayAddress = ipaddress.Substring(0, ipaddress.LastIndexOf("."));
                    return gateWayAddress + ".1";
                }
                else
                    return "";
            }

            static public uint ReturnFirtsOctet(string ipAddress)

            {
                //IPAddress IP = IPAddress.Parse(ipAddress);
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

        public void saveUserConfig()
        {

            UserDefinedSaves.Default.konfig1ip = conf1IP.Text;
            UserDefinedSaves.Default.konfig1subnet = conf1Subnet.Text;
            UserDefinedSaves.Default.konfig1gateway = conf1Gateway.Text;

            UserDefinedSaves.Default.konfig2ip = conf2IP.Text;
            UserDefinedSaves.Default.konfig2subnet = conf2Subnet.Text;
            UserDefinedSaves.Default.konfig2gateway = conf2Gateway.Text;

            UserDefinedSaves.Default.konfig3ip = conf3IP.Text;
            UserDefinedSaves.Default.konfig3subnet = conf3Subnet.Text;
            UserDefinedSaves.Default.konfig3gateway = conf3Gateway.Text;

            UserDefinedSaves.Default.konfig4ip = conf4IP.Text;
            UserDefinedSaves.Default.konfig4subnet = conf4Subnet.Text;
            UserDefinedSaves.Default.konfig4gateway = conf4Gateway.Text;

            UserDefinedSaves.Default.konfig5ip = conf5IP.Text;
            UserDefinedSaves.Default.konfig5subnet = conf5Subnet.Text;
            UserDefinedSaves.Default.konfig5gateway = conf5Gateway.Text;
        }
        public void loadUserConfig()
        {
            conf1IP.Text = UserDefinedSaves.Default.konfig1ip;
            conf1Subnet.Text = UserDefinedSaves.Default.konfig1subnet;
            conf1Gateway.Text = UserDefinedSaves.Default.konfig1gateway;

            conf2IP.Text = UserDefinedSaves.Default.konfig2ip;
            conf2Subnet.Text = UserDefinedSaves.Default.konfig2subnet;
            conf2Gateway.Text = UserDefinedSaves.Default.konfig2gateway;

            conf3IP.Text = UserDefinedSaves.Default.konfig3ip;
            conf3Subnet.Text = UserDefinedSaves.Default.konfig3subnet;
            conf3Gateway.Text = UserDefinedSaves.Default.konfig3gateway;

            conf4IP.Text = UserDefinedSaves.Default.konfig4ip;
            conf4Subnet.Text = UserDefinedSaves.Default.konfig4subnet;
            conf4Gateway.Text = UserDefinedSaves.Default.konfig4gateway;

            conf5IP.Text = UserDefinedSaves.Default.konfig5ip;
            conf5Subnet.Text = UserDefinedSaves.Default.konfig5subnet;
            conf5Gateway.Text = UserDefinedSaves.Default.konfig5gateway;
        }

        public string subnetAutoComplete(string IPTextbox)
        {
            return IPClassTester.GetSubnetMask(IPTextbox);

        }

        private void On_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveUserConfig();
            UserDefinedSaves.Default.Save();
        }

        private void config1_ip_updated(object sender, RoutedEventArgs e)
        {
            conf1Subnet.Text = subnetAutoComplete(conf1IP.Text);
            conf1Gateway.Text = IPClassTester.gatewayAutoComplete(conf1IP.Text);
        }

        private void config2_ip_updated(object sender, RoutedEventArgs e)
        {
            conf2Subnet.Text = subnetAutoComplete(conf2IP.Text);
            conf2Gateway.Text = IPClassTester.gatewayAutoComplete(conf2IP.Text);
        }

        private void config3_ip_updated(object sender, RoutedEventArgs e)
        {
            conf3Subnet.Text = subnetAutoComplete(conf3IP.Text);
            conf3Gateway.Text = IPClassTester.gatewayAutoComplete(conf3IP.Text);
        }

        private void config4_ip_updated(object sender, RoutedEventArgs e)
        {
            conf4Subnet.Text = subnetAutoComplete(conf4IP.Text);
            conf4Gateway.Text = IPClassTester.gatewayAutoComplete(conf4IP.Text);
        }

        private void config5_ip_updated(object sender, RoutedEventArgs e)
        {
            conf5Subnet.Text = subnetAutoComplete(conf5IP.Text);
            conf5Gateway.Text = IPClassTester.gatewayAutoComplete(conf5IP.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            setIP(conf1IP.Text,conf1Subnet.Text,conf1Gateway.Text);
        }
    }
}
