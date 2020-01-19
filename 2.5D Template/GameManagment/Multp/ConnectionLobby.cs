using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class ConnectionLobby : Connection
{
    public List<PlayerList> playerlists = new List<PlayerList>();
    public List<int> portlist = new List<int>();
    public List<int> inactivitytimer = new List<int>();
    float time;

    public ConnectionLobby(int port)
        : base(port)
    {
        ar_ = udpclient.BeginReceive(Receive, new object());
        Console.WriteLine("Created lobby connection");
    }

    private void Receive(IAsyncResult ar)
    {
        try //if connection closes catches errors
        {
            byte[] bytes = udpclient.EndReceive(ar, ref remoteep); //store received data in byte array

            if (remoteep.Address.ToString() != MyIP().ToString()) //check if we did not receive from local ip (we dont need our own data) 
            {
                string message = Encoding.ASCII.GetString(bytes); //convert byte array to string
                Console.WriteLine("re " + message);
                HandleReceivedData(message, remoteep.Address);
            }
            ar_ = udpclient.BeginReceive(Receive, new object()); ; //repeat
        }
        catch
        {

        }
    }

    public void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (time >= 1)
        {
            for (int i = 0; i < playerlists.Count; i++)
            {
                inactivitytimer[i]++;
            }
            for (int i = 0; i < playerlists.Count; i++)
            {
                if (inactivitytimer[i] >= 5)
                {
                    playerlists.RemoveAt(i);
                    portlist.RemoveAt(i);
                    inactivitytimer.RemoveAt(i);
                }
            }
            time = 0;
        }
    }

    public void HandleReceivedData(string message, IPAddress sender) //inspect received data and take action
    {
        string[] variables = message.Split(' ');
        if (variables[0] == "Playerlist")
        {
            if (playerlists.Count == 0)
            {
                playerlists.Add(new PlayerList());
                portlist.Add(int.Parse(variables[1]));
                inactivitytimer.Add(0);
                playerlists[0].Store(message);
            }
            else
            {
                int count = 0;
                bool newplayerlist = true;
                foreach (PlayerList playerlist in playerlists)
                {
                    if (playerlist.IsHost(sender))
                    {
                        newplayerlist = false;
                        inactivitytimer[count] = 0;
                        playerlist.Store(message);
                        break;
                    }
                    count++;
                }
                if (newplayerlist) //ip was not yet in the list
                {
                    playerlists.Add(new PlayerList());
                    portlist.Add(int.Parse(variables[1]));
                    inactivitytimer.Add(0);
                    playerlists[playerlists.Count - 1].Store(message);
                    GameEnvironment.GameStateManager.GetGameState("hostSelectionState");
                }
            }
        }
        else if (variables[0] == "Closed:")
        {
            string[] parts = variables[1].Split(':');
            ListsRemoveAt(IPAddress.Parse(parts[0]));
        }
    }

    private void ListsRemoveAt(IPAddress ip)
    {
        for (int i = 0; i < playerlists.Count; i++)
        {
            if (playerlists[i].IsHost(ip))
            {

                playerlists.RemoveAt(i);
                portlist.RemoveAt(i);
                inactivitytimer.RemoveAt(i);
                break;
            }
        }
    }

    public void Disconnect() //stop receiving and sending data
    {
        udpclient.Close();
        MultiplayerManager.Lobby = null;
        Console.WriteLine("Disconnect from Lobby");
    }
}
