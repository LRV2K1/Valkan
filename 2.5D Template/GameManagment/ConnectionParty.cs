﻿using System.Collections.Generic;
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
            if (ip.Address.ToString() != MyIP().ToString()) //check if we did not receive from local ip (we dont need our own data) 
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
        if (playerlist.IsHost(MyIP()) && time > 3)
        {
            Send(playerlist.PlayerListToString(), 1000); //broadcast playerlist to port 1000
            time = 0;
        }
    }

    public void HandleReceivedData(string message, IPAddress sender) //inspect received data and take action
    {
        //todo if playing
        if (playerlist.IsHost(MyIP()))
        {
            if (message == "Join")
            {
                playerlist.Modify(sender);
                Send(playerlist.PlayerListToString(), 1000);
                Send(playerlist.PlayerListToString(), port);
            }
            else if (message == "Leave")
            {
                playerlist.Modify(sender, false, false, true);
            }
            else if (message == "Ready")
            {
                playerlist.Modify(sender, true);
                Send(playerlist.PlayerListToString(), 1000);
                Send(playerlist.PlayerListToString(), port);
            }
            else
            {
                Console.WriteLine("ERROR! The message:\n" + message + "\nis not a valid message");
            }
        }
        else
        {
            string[] lines = message.Split('\n');
            if (lines[0] == "Playerlist:")
            {
                playerlist.Store(message);
            }
            else if (message == "HostLeaves") //if the host leaves then disconnect and go back to portselectionstate
            {
                Disconnect();
                GameEnvironment.ScreenFade.TransitionToScene("portSelectionState", 5);
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
        }
        else
        {
            Send("Leave", 9999);
        }
        client.Close();
        MultiplayerManager.party = null;
        Console.WriteLine("Disconnect from party");
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