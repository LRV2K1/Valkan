using System.Collections.Generic;
using System.Net;
using System;

public class PlayerList
{
    public List<LobbyPlayer> playerlist;
    public PlayerList()
    {
        playerlist = new List<LobbyPlayer>();
    }

    public void Store(string message)
    {
        string[] lines = message.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            //variables[0] = ip
            //variables[1] = isready
            //variables[2] = ishost
            //variables[3] = character
            string[] variables = lines[i].Split(new string[] { ", " }, StringSplitOptions.None);
            Modify(IPAddress.Parse(variables[0]), bool.Parse(variables[1]), bool.Parse(variables[2]), character: variables[3]); //modify playerlist with this data
        }
    }
    public void Modify(IPAddress ip, bool isready = false, bool ishost = false, bool leave = false, string character = "Warrior")
    {
        int count = 0;
        bool newplayer = true;
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            if (lobbyplayer.ip == ip) //is this player already in the playerlist
            {
                newplayer = false;
                if (leave) //is this player leaving
                {
                    playerlist.RemoveAt(count);
                }
                else if (isready) //modify otherwise
                {
                    lobbyplayer.isready = isready;
                }
                else
                {
                    lobbyplayer.character = character;
                }
            }
            count++;
        }
        if (newplayer)
        {
            playerlist.Add(new LobbyPlayer(ip, ishost));
        }
    }

    public bool IsHost(IPAddress ip)
    {
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            if (lobbyplayer.ip.ToString() == ip.ToString()) //is this player already in the playerlist
            {
                return lobbyplayer.ishost;
            }
        }
        return false;
    }

    public string PlayerListToString()
    {
        string message = "Playerlist:";
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            message += "\n" + lobbyplayer.ip.ToString() + ", " + lobbyplayer.isready + ", " + lobbyplayer.ishost;
        }
        return message;
        //Playerlist: 
        //196.168.21.4, false, true
        //196.168.21.24, false, false
        //196.168.21.3, false, false
    }

    public bool AllReady()
    {
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            if (!lobbyplayer.ishost && !lobbyplayer.isready) //is someone is not ready and not host
            {
                return false;
            }
        }
        return true;
    }
}