using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

//this class has the key methods in it like Send(), MYIP(), etc.
public partial class Connection
{
    protected int port;
    protected string data = "";
    protected UdpClient udpclient;
    protected IPAddress multicastaddress;
    protected IPEndPoint remoteep;
    protected IPEndPoint localEp;
    protected IAsyncResult ar_ = null;

    public Connection(int port)
    {
        this.port = port;
        multicastaddress = IPAddress.Parse("239.0.0.222");
        udpclient = new UdpClient();
        remoteep = new IPEndPoint(multicastaddress, port);
        localEp = new IPEndPoint(IPAddress.Any, port);

        udpclient.JoinMulticastGroup(multicastaddress);
        udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.MulticastTimeToLive, 0,);
        udpclient.ExclusiveAddressUse = false;
        udpclient.Client.Bind(localEp);
        Send("Send first message", port, false);
    }

    public void Send(string message, int port, bool log = true) //convert string to bytes to broadcast it
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        udpclient.Send(bytes, bytes.Length, new IPEndPoint(multicastaddress, port));
        if (log) //should the send message be put in console
        {
            Console.WriteLine("\nSent on port " + port + " ->\n{0}", message);
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

    public string GetReceivedData()
    {
        return data;
    }
}