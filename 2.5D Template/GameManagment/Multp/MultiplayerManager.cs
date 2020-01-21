using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;

//Send() and GetReceivedData() are the main 2 methods of this class
public class MultiplayerManager
{
    static ConnectionParty party;
    static ConnectionLobby lobby;
    static int lobbyport = 1900;
    static int partyport;
    static bool online = false;

    public static int LobbyPort { get { return lobbyport; } }
    public static int PartyPort { get { return partyport; } }
    static public bool Online { get { return online; } }
    public static ConnectionParty Party { get { return party; } set { party = value; } }
    public static ConnectionLobby Lobby { get { return lobby; } set { lobby = value; } }

    public MultiplayerManager()
    {
    }

    //setup connection for lobby or ingame
    public static void Connect(int port = 0)
    {
        if (port == LobbyPort)
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
                if (port == 0)
                {
                    int randomport = GameEnvironment.Random.Next(2000, 60000);
                    party = new ConnectionParty(randomport);
                    partyport = randomport;
                }
                else
                {
                    party = new ConnectionParty(port);
                    partyport = port;
                }
            }
            else
            {
                Console.WriteLine("Already connected to party port " + PartyPort);
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        if (party != null)
        {
            party.Update(gameTime);
        }
        if (lobby != null)
        {
            lobby.Update(gameTime);
        }
        if (party == null)
        {
            online = false;
        }
        else
        {
            online = true;
        }
    }
}