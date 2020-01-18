using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Net.NetworkInformation;

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
        this.port = port;
        client = new UdpClient(port);
        ip = new IPEndPoint(IPAddress.Any, port);
    }
    private IPAddress GetBroadCastIP()
    {
        try
        {
            string ipadress;
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); // get a list of all local IPs
            IPAddress localIpAddress = ipHostInfo.AddressList[0]; // choose the first of the list
            ipadress = Convert.ToString(localIpAddress); // convert to string
            ipadress = ipadress.Substring(0, ipadress.LastIndexOf(".") + 1); // cuts of the last octet of the given IP 
            ipadress += "255"; // adds 255 witch represents the local broadcast
            return IPAddress.Parse(ipadress);
        }
        catch (Exception e)
        {
            return IPAddress.Parse("127.0.0.1");// in case of error return the local loopback
        }
    }


    public string GetReceivedData()
    {
        return data;
    }
    public void Send(string message, int port) //convert string to bytes to broadcast it
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        client.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Broadcast, port)); //broadcast to specific port
        Console.WriteLine("\nSent on " + IPAddress.Broadcast.ToString() + ":" + port + " ->\n{0}", message);
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