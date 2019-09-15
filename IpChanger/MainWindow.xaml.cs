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
            LoadUserConfig();
            GetInterfaces();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentInformations();
        }

        public void GetCurrentInformations()
        {
            NetworkInterface selectedNetworkInterface = GetSelectedNetworkInterface();

            currentIpAddress.Text = IpHelper.GetLocalIPAddress(selectedNetworkInterface);
            currentSubnetMask.Text = IpHelper.GetSubnetMask(currentIpAddress.Text);
            currentDefaultGateway.Text = IpHelper.GetDefaultGateway(selectedNetworkInterface);
        }

        public void SaveUserConfig()
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
        public void LoadUserConfig()
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

        private void On_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveUserConfig();
            UserDefinedSaves.Default.Save();
        }

        private void Config1_ip_updated(object sender, RoutedEventArgs e)
        {
            conf1Subnet.Text = IpHelper.SubnetAutoComplete(conf1IP.Text);
            conf1Gateway.Text = IpHelper.GatewayAutoComplete(conf1IP.Text);
        }

        private void Config2_ip_updated(object sender, RoutedEventArgs e)
        {
            conf2Subnet.Text = IpHelper.SubnetAutoComplete(conf2IP.Text);
            conf2Gateway.Text = IpHelper.GatewayAutoComplete(conf2IP.Text);
        }

        private void Config3_ip_updated(object sender, RoutedEventArgs e)
        {
            conf3Subnet.Text = IpHelper.SubnetAutoComplete(conf3IP.Text);
            conf3Gateway.Text = IpHelper.GatewayAutoComplete(conf3IP.Text);
        }

        private void Config4_ip_updated(object sender, RoutedEventArgs e)
        {
            conf4Subnet.Text = IpHelper.SubnetAutoComplete(conf4IP.Text);
            conf4Gateway.Text = IpHelper.GatewayAutoComplete(conf4IP.Text);
        }

        private void Config5_ip_updated(object sender, RoutedEventArgs e)
        {
            conf5Subnet.Text = IpHelper.SubnetAutoComplete(conf5IP.Text);
            conf5Gateway.Text = IpHelper.GatewayAutoComplete(conf5IP.Text);
        }

        private void Activate_conf1(object sender, RoutedEventArgs e)
        {
            if (conf1IP.Text != "")
            {
                IpHelper.SetIP(conf1IP.Text, conf1Subnet.Text, conf1Gateway.Text, GetSelectedNetworkInterface().Name.ToString());
            }
        }

        private void Activate_conf2(object sender, RoutedEventArgs e)
        {
            if (conf2IP.Text != "")
            {
                IpHelper.SetIP(conf2IP.Text, conf2Subnet.Text, conf2Gateway.Text, GetSelectedNetworkInterface().Name.ToString());
            }
        }

        private void Activate_conf3(object sender, RoutedEventArgs e)
        {
            if (conf3IP.Text != "")
            {
                IpHelper.SetIP(conf3IP.Text, conf3Subnet.Text, conf3Gateway.Text, GetSelectedNetworkInterface().Name.ToString());
            }
        }

        private void Activate_conf4(object sender, RoutedEventArgs e)
        {
            if (conf4IP.Text != "")
            {
                IpHelper.SetIP(conf4IP.Text, conf4Subnet.Text, conf4Gateway.Text, GetSelectedNetworkInterface().Name.ToString());
            }
        }

        private void Activate_conf5(object sender, RoutedEventArgs e)
        {
            if (conf5IP.Text != "")
            {
                IpHelper.SetIP(conf5IP.Text, conf5Subnet.Text, conf5Gateway.Text, GetSelectedNetworkInterface().Name.ToString());
            }
        }

        public void GetInterfaces()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                networkinterfaceList.Items.Add(adapter);
            }
            networkinterfaceList.SelectedIndex = 0;
        }

        public NetworkInterface GetSelectedNetworkInterface() => (NetworkInterface)networkinterfaceList.SelectedItem;
    }
}
