using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        if (connection != null)
        {
            if (inputHelper.KeyPressed(Keys.M)) //disconnect
            {
                connection.Stop();
                connection = null;
                connection.Send("Close");
            }
        }
        else //if there is no connection:
        {
            if (inputHelper.KeyPressed(Keys.N)) //Create a new game
            {
                connection = new Connection();


                //TODO
                //1 load level
                //TODO


                Console.WriteLine("Setup Connection");
            }
            if (inputHelper.KeyPressed(Keys.C)) //Connect to a currently running game
            {
                connection = new Connection();
                //Level.LoadLevel(connection.GetWorld());
                using (StreamWriter sw = File.AppendText("Content/Levels/level_1.txt"))
                {
                    Console.WriteLine("3");
                    sw.WriteLine("hi");
                }

                //TODO
                //get level
                //1 Create new txt file
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