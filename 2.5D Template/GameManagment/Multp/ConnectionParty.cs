using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using Microsoft.Xna.Framework;
using System.IO;
using System.Text;

public class ConnectionParty : Connection
{
    public PlayerList playerlist;
    float time;

    public ConnectionParty(int port)
        : base(port)
    {
        playerlist = new PlayerList();
        ar_ = client.BeginReceive(Receive, new object());
        Console.WriteLine("Created party connection");
    }

    private void Receive(IAsyncResult ar)
    {
        try
        {
            byte[] bytes = client.EndReceive(ar, ref ip); //store received data in byte array
            if (ip.Address.ToString() != MyIP().ToString()) //check if we did not receive from own ip (we dont need our own data) 
            {
                string message = Encoding.ASCII.GetString(bytes); //convert byte array to string
                Console.WriteLine("\nReceived from {1}:" + port + " ->\n{0}", message, ip.Address.ToString());
                HandleReceivedData(message, ip.Address);
                data = message;
            }
            ar_ = client.BeginReceive(Receive, new object()); ; //repeat
        }
        catch
        {
        }
    }
    public void Update(GameTime gameTime)
    {
        //if currentgamestate is playing TODO
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (playerlist.IsHost(MyIP()) && time > 1)
        {
            if (playerlist.playerlist.Count > 1) //do only if host is not alone in a party
            {
                Send("Host is still connected", port); //message send by host only, if host crashes the clients wont be stuck
                playerlist.Modify(MyIP(), timeunactive: 0);
                foreach (LobbyPlayer lobbyplayer in playerlist.playerlist)
                {
                    lobbyplayer.timeunactive += 1;
                    if (lobbyplayer.timeunactive >= 5)
                    {
                        playerlist.Modify(lobbyplayer.ip, false, false, true);
                        Send("Playerlist:" + playerlist.ToString(), port);
                        break;
                    }
                }
            }            
            Send("Playerlist " + port + " :" + playerlist.ToString(), 1000); //broadcast playerlist to port 1000
            time = 0;
        }
        else if (time > 1) //not host
        {
            Send("I am still connected", port); //message send by clients, this prevents error when a client types alt + f4.
            playerlist.Modify(MyIP(), timeunactive: 0);
            foreach (LobbyPlayer lobbyplayer in playerlist.playerlist)
            {
                if (lobbyplayer.ishost)
                {
                    lobbyplayer.timeunactive += 1;
                    if (lobbyplayer.timeunactive >= 5)
                    {
                        Disconnect();
                    }
                }
            }
            time = 0;
        }
    }

    public void HandleReceivedData(string message, IPAddress sender) //inspect received data and take action
    {
        //todo if playing
        string[] lines = message.Split('\n');
        string[] variables = message.Split(' ');
        if (playerlist.IsHost(MyIP())) //data for host only
        {
            if (message == "Join")
            {
                playerlist.Modify(sender);
                Send("Playerlist:" + playerlist.ToString(), port);
            }
            else if (message == "Leave")
            {
                playerlist.Modify(sender, false, false, true);
                Send("Playerlist:" + playerlist.ToString(), port);
            }
            else if (message == "Ready")
            {
                playerlist.Modify(sender, true);
                Send("Playerlist:" + playerlist.ToString(), port);
            }
            else if (variables[0] == "Character:")
            {
                playerlist.Modify(sender, character: variables[1]);
            }
            else if (lines[0] == "Playerlist:")
            {
                playerlist.Store(message);
            }
            else if (message == "I am still connected")
            {
                playerlist.Modify(sender, timeunactive: 0);
            }
            else
            {
                Console.WriteLine("ERROR! The message:\n" + message + "\nis not a valid message");
            }
        }
        else //data for everyone but host
        {
            if (lines[0] == "Playerlist:")
            {
                playerlist.Store(message);
            }
            else if (message == "HostLeaves") //if the host leaves then disconnect and go back to portselectionstate
            {
                Disconnect();
                GameEnvironment.ScreenFade.TransitionToScene("portSelectionState", 5);
                MultiplayerManager.Connect(1000);
                MultiplayerManager.party = null;
            }
            else if (message == "Host is still connected")
            {
                playerlist.Modify(sender, timeunactive: 0);
                if (true) //send only by player2
                {
                    Send("Playerlist:" + playerlist.ToString(), port);
                }
            }
            else
            {
                Console.WriteLine("ERROR! The message:\n" + message + "\nis not a valid message");
            }
        }
    }

    public void Disconnect() //stop receiving and sending data
    {
        if (playerlist.IsHost(MyIP()))
        {
            Send("HostLeaves", 9999);
            Send("Closed: " + MyIP().ToString() + ":" + port, 1000);
        }
        else
        {
            Send("Leave", 9999);
        }
        client.Close();
        Console.WriteLine("Disconnect from party");
        MultiplayerManager.party = null;
    }
    private void StoreWorld(string file, string world) //write to <file>.txt from a single string containing the world
    {
        string path = "Content/Levels/" + file + ".txt";
        StreamWriter writer = new StreamWriter(path, false);
        string[] lines = world.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            writer.WriteLine(lines[i]);
        }
        Console.WriteLine("Successfully wrote " + (lines.Length - 1) + " lines to " + path);
        writer.Close();
    }
    public string WorldToString(string file) //convert <file>.txt to string
    {
        string message = "";
        string path = "Content/Levels/" + file + ".txt";
        StreamReader streamReader = new StreamReader(path);
        string line = streamReader.ReadLine();
        for (int i = 0; i < 4; i++)
        {
            while (line != null)
            {
                message += line + "\n";
                line = streamReader.ReadLine();
            }
        }
        streamReader.Close();
        return message;
    }
}