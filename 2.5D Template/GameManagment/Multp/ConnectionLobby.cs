using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class ConnectionLobby : Connection
{
    public List<PlayerList> playerlists = new List<PlayerList>();
    public List<int> portlist = new List<int>();

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
                Console.WriteLine("\nReceived from {1}" + port + " ->\n{0}", message, ip.Address.ToString());
                HandleReceivedData(message, ip.Address, ip.Port);
            }
            ar_ = client.BeginReceive(Receive, new object()); ; //repeat
        }
        catch
        {

        }
    }

    public void HandleReceivedData(string message, IPAddress sender, int port) //inspect received data and take action
    {
        string[] variables = message.Split(' ');
        if (variables[0] == "Playerlist")
        {
            if (playerlists.Count == 0)
            {
                playerlists.Add(new PlayerList());
                portlist.Add(int.Parse(variables[1]));
                playerlists[0].Store(message);
            }
            else
            {
                foreach (PlayerList playerlist in playerlists)
                {
                    if (playerlist.IsHost(sender))
                    {
                        playerlist.Store(message);
                        break;
                    }
                    else //this ip does not exist so create new playerlist
                    {
                        playerlists.Add(new PlayerList());
                        portlist.Add(int.Parse(variables[1]));
                        playerlists[playerlists.Count - 1].Store(message);
                        GameEnvironment.GameStateManager.GetGameState("hostSelectionState");
                        break;
                    }
                }
            }
        }
        else if (variables[0] == "Closed:")
        {
            string[] parts = variables[1].Split(':');
            for (int i = 0; i < playerlists.Count; i++)
            {
                if (playerlists[i].IsHost(IPAddress.Parse(parts[0])))
                {

                    playerlists.RemoveAt(i);
                    portlist.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void Disconnect() //stop receiving and sending data
    {
        client.Close();
        MultiplayerManager.lobby = null;
        Console.WriteLine("Disconnect from Lobby");
    }
}
