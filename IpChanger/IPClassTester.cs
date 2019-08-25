using System.Net;

namespace IpChanger
{
    public partial class MainWindow
    {
        class IPClassTester

        {
            public static string GetSubnetMask(string ipaddress)
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

            public static uint ReturnFirtsOctet(string ipAddress)

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
}
