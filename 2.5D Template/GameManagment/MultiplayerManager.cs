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

    public void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.K)) //sendworld
        {
            connection.Send(connection.WorldToString("Online"));
        }

        if (connection != null)
        {
            if (inputHelper.KeyPressed(Keys.M)) //disconnect
            {
                connection.Disconnect();
                connection = null;
                connection.Send("Close");
            }
        }
        else //if there is no connection:
        {
            if (inputHelper.KeyPressed(Keys.N)) //Create a new game
            {
                connection = new Connection();
                connection.playerlist.Add(Connection.MyIP()); //add our list with our own ip
                //level = new Level("Level_1");
                Console.WriteLine("Created Game");


                Console.WriteLine("Setup Connection");
            }
            if (inputHelper.KeyPressed(Keys.C)) //Connect to a currently running game
            {
                connection = new Connection();
                connection.Send("GetPlayerList: all");
                connection.playerlist.Add(Connection.MyIP()); //add our list with our own ip
                connection.Send("AddToPlayerList");
                connection.Send("GetWorld: Online");
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
    }
}