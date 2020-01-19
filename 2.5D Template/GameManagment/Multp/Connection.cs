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
    public int port;
    public string data = "";
    protected UdpClient udpclient;
    protected IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
    protected IPEndPoint remoteep;
    protected IPEndPoint localEp;
    protected IAsyncResult ar_ = null;

    public Connection(int port)
    {
        this.port = port;
        udpclient = new UdpClient();
        remoteep = new IPEndPoint(multicastaddress, port);
        localEp = new IPEndPoint(IPAddress.Any, port);

        udpclient.JoinMulticastGroup(multicastaddress);
        udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udpclient.ExclusiveAddressUse = false;
        udpclient.Client.Bind(localEp);
        Send("Send first message", port, false);
    }
   

    public string GetReceivedData()
    {
        return data;
    }
    public void Send(string message, int port, bool log = true) //convert string to bytes to broadcast it
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        udpclient.Send(bytes, bytes.Length, new IPEndPoint(multicastaddress, port));
        if (log) //should the send message be put in console
        {
            Console.WriteLine("\nSentttt on " + remoteep.ToString() + ":" + port + " ->\n{0}", message);
        }
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