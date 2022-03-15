using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace gps_mock_app.Controllers
{
    public class UdpController
    {
        public UdpController()
        {

        }

        public static void SendMessage(string message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.0.255"), 2201);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
            Console.WriteLine("Sent: {0} ", message);
        }            
    }
}
