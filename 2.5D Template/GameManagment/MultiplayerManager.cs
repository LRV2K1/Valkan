using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;

//Send() and GetReceivedData() are the main 2 methods of this class
public class MultiplayerManager
{
    public static Connection connection;

    public MultiplayerManager()
    {
    }


    public static void Send(string data)
    {
        if (connection != null)
        {
            connection.Send(data);
        }
    }

    public static string GetReceivedData()
    {
        if (connection != null)
        {
            return connection.GetReceivedData();
        }
        return null;
    }

    public static void SetupClient()
    {
        if (connection == null)
        {
            connection = new Connection();
            connection.Send("GetPlayerList: all");
            while (GetReceivedData() != "NICE")
            {

            }
            //check for recehived

            connection.playerlist.Add(Connection.MyIP()); //add our list with our own ip
            connection.Send("Playerlist: " + connection.PlayerListToString());
            while (GetReceivedData() != "NICE")
            {

            }
            connection.Send("GetWorld: Online");
            while (GetReceivedData() != "NICEWORLD")
            {

            }
            //level = new Level("Online");
            //TODO
            //get level
            //1 p
            //2 receive data from running game
            //2a let new player send GetLevel() to someone with a running game
            //2b someone who is playing sends the level
            //2c new player receives the level
            //3 put data in txt file
            //4 load the new txt file level
            //TODO


            Console.WriteLine("Setup Connection");
        }
    }
    
    public static void SetupHost()
    {
        if (connection == null)
        {
            connection = new Connection();
            connection.playerlist.Add(Connection.MyIP()); //add our list with our own ip
        }
    }
    public static void Disconnect()
    {
        if (connection != null)
        {
            connection.Disconnect();
            connection = null;
            connection.Send("Close");
        }
    }

    public void HandleInput(InputHelper inputHelper)
    {
    }
}