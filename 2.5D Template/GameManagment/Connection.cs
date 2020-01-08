using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class Connection
{
    protected SpriteFont spriteFont;
    const int port = 15000;
    UdpClient client = new UdpClient(port);
    IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
    IAsyncResult ar_ = null;
    string data = "";

    public Connection()
    {
        Console.WriteLine("Started listening");
        StartListening();
    }


    public void Stop()
    {
        client.Close();
        Console.WriteLine("Stopped listening");
    }

    private void StartListening()
    {
        ar_ = client.BeginReceive(Receive, new object());
    }


    public void GetWorld()
    {

    }
    public void SendWorld()
    {
        //string message = 
        //byte[] bytes = Encoding.ASCII.GetBytes(message);
    }
    public void Receive(IAsyncResult ar)
    {
        try
        {
            byte[] bytes = client.EndReceive(ar, ref ip); //store received data in byte array

            if (ip.Address.ToString() != MyIP()) //check if we did not receive from local ip (we dont need our own data) 
            {
                string message = Encoding.ASCII.GetString(bytes); //convert byte array to string
                string[] variables = message.Split(' '); //split data in ID, posX, posY respectively
                data = message;
                Console.WriteLine("Received: {0}, from: {1} ", message, ip.Address.ToString());
            }
            StartListening(); //repeat
        }
        catch
        {
        }
    }
    public string GetReceivedData()
    {
        return data;
    }
    public void Send(string message) //broadcast
    {
        byte[] bytes = Encoding.ASCII.GetBytes(message);
        client.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("255.255.255.255"), port));
        Console.WriteLine("Sent: {0}", message);
    }

    public static string MyIP()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system");
    }
}
