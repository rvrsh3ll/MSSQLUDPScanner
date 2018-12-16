using System;
using System.Text;
using NDesk.Options;
using LukeSkywalker.IPNetwork;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MSSQLUDPScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            string cidr = null;
            bool showhelp = false;

            var opts = new OptionSet()
            {
                { "cidr=", " --cidr 192.168.1.0/24", v => cidr = v },
                { "h|?|help",  "Show available options", v => showhelp = v != null },
            };
            try
            {
                opts.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
            }
            if (showhelp)
            {
                Console.WriteLine("RTFM");
                opts.WriteOptionDescriptions(Console.Out);
                Console.WriteLine("[*] Example: MSSQLUDPScanner.exe --cidr 192.168.1.0/24");
                return;
            }

            // Parse CIDR
            IPNetwork ipn = IPNetwork.Parse(cidr);
            IPAddressCollection ips = IPNetwork.ListIPAddress(ipn);
            Parallel.ForEach(ips, ip =>
            {
                try
                {
                    var udpClient = new UdpClient();
                    IPEndPoint ep = new IPEndPoint((ip), 1434);
                    udpClient.Connect(ep);
                    int number = 3;
                    byte[] sendBytes = BitConverter.GetBytes(number);
                    udpClient.Client.ReceiveTimeout = 10000;
                    udpClient.Client.Blocking = true;
                    udpClient.Send(sendBytes, sendBytes.Length);
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);
                    string results = returnData.ToString();
                    Console.WriteLine("IP: " + ip + " " + results.Replace(';', ' ') + "\r\n");
                    udpClient.Close();

                }
                catch (Exception)
                {
                    // Do not print errors
                }
            });
        }
    }
}
