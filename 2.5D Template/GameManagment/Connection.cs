using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

//this class has the key methods in it like Send(), Receive(), Disconnect(), etc.
public partial class Connection
{
    protected int port;
    public UdpClient client;
    protected IPEndPoint ip;
    protected IAsyncResult ar_ = null;
    public string data = "";

    public Connection(int port)
    {
        client = new UdpClient(port);
        ip = new IPEndPoint(IPAddress.Any, port);
    }

    public string GetReceivedData()
    {
        return data;
    }
    public void Send(string message, int port) //convert string to bytes to broadcast it
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        client.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("255.255.255.255"), port)); //broadcast to specific port
        Console.WriteLine("\nSent on " + port + " ->\n{0}", message);
    }

    public static IPAddress MyIP() //returns own IP address
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip;
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system");
    }
}
