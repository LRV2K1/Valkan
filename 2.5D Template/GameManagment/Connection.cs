using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

//this class has the key methods in it like Send(), Receive(), Disconnect(), etc.
public partial class Connection
{
    const int port = 15020;
    UdpClient client = new UdpClient(port);
    IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
    IAsyncResult ar_ = null;
    public List<IPAddress> playerlist = new List<IPAddress>();

    public string data = "Action: ID x y";
    public string somedata = "";

    public Connection()
    {
        ar_ = client.BeginReceive(Receive, new object());
        Console.WriteLine("Started listening");
    }

    public void Send(string message) //convert string to bytes to broadcast it
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        client.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("255.255.255.255"), port)); //broadcast to specific port
        Console.WriteLine("\nSent->\n{0}", message);
    }

    private void Receive(IAsyncResult ar)
    {
        try
        {
            byte[] bytes = client.EndReceive(ar, ref ip); //store received data in byte array

            if (ip.Address.ToString() != MyIP().ToString()) //check if we did not receive from local ip (we dont need our own data) 
            {
                string message = Encoding.ASCII.GetString(bytes); //convert byte array to string
                Console.WriteLine("\nReceived from {1}->\n{0}", message, ip.Address.ToString());
                HandleReceivedData(message);
            }
            ar_ = client.BeginReceive(Receive, new object()); ; //repeat
            //WARNING StartListening might need to be put in Entity.cs after the GetReceivedData method
            //WARNING StartListening might need to be put in Entity.cs after the GetReceivedData method
            //WARNING StartListening might need to be put in Entity.cs after the GetReceivedData method
        }
        catch
        {

        }
    }

    public void Disconnect()
    {
        client.Close();
        Console.WriteLine("Stopped listening");
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
