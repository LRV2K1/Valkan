using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;

//Send() and GetReceivedData() are the main 2 methods of this class
public class MultiplayerManager
{
    public static ConnectionParty party;
    public static ConnectionLobby lobby;
    public static bool online = false;
    int count = 0;
    public MultiplayerManager()
    {
    }

    //setup connection for lobby or ingame
    public static void Connect(int port)
    {
        if (port == 1000)
        {
            if (lobby == null)
            {
                lobby = new ConnectionLobby(port);
            }
            else
            {
                Console.WriteLine("Already connected to port " + port);
            }
        }
        else
        {
            if (party == null)
            {
                party = new ConnectionParty(port);
            }
            else
            {
                Console.WriteLine("Already connected to port " + port);
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        if (party != null)
        {
            party.Update(gameTime);
        }
        if (party == null && lobby == null)
        {
            online = false;
        }
        else
        {
            online = true;
        }
    }
}