using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class ConnectionLobby : Connection
{
    public PlayerList playerlist;

    public ConnectionLobby(int port)
        : base(port)
    {
        ar_ = client.BeginReceive(Receive, new object());
        Console.WriteLine("Created lobby connection");
    }

    private void Receive(IAsyncResult ar)
    {
        try
        {
            byte[] bytes = client.EndReceive(ar, ref ip); //store received data in byte array

            if (ip.Address.ToString() != MyIP().ToString()) //check if we did not receive from local ip (we dont need our own data) 
            {
                string message = Encoding.ASCII.GetString(bytes); //convert byte array to string
                Console.WriteLine("\nReceived from {1}:" + port + " ->\n{0}", message, ip.Address.ToString());
                HandleReceivedData(message);
            }
            ar_ = client.BeginReceive(Receive, new object()); ; //repeat
        }
        catch
        {

        }
    }

    public void HandleReceivedData(string message) //inspect received data and take action
    {
        string[] lines = message.Split('\n');
        if (lines[0] == "Playerlist:")
        {
            playerlist.Store(message);
        }
    }

    public void Disconnect() //stop receiving and sending data
    {
        client.Close();
        MultiplayerManager.lobby = null;
        Console.WriteLine("Disconnect from Lobby");
    }
}
