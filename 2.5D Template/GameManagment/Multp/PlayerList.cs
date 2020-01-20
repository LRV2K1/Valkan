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
            //variables[4] = timeunactive
            //variables[5] = receivedworld
            string[] variables = lines[i].Split(new string[] { ", " }, StringSplitOptions.None);
            Modify(IPAddress.Parse(variables[0]), bool.Parse(variables[1]), bool.Parse(variables[2]), character: variables[3], timeunactive: float.Parse(variables[4]), receivedworld: bool.Parse(variables[5])); //modify playerlist with this data
        }
    }
    public void Modify(IPAddress ip, bool isready = false, bool ishost = false, bool leave = false, string character = "null", float timeunactive = 0, bool receivedworld = false)
    {
        int count = 0;
        bool newplayer = true;
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            if (lobbyplayer.ip.ToString() == ip.ToString()) //is this player already in the playerlist
            {
                newplayer = false;
                if (leave) //is this player leaving
                {
                    playerlist.RemoveAt(count);
                    Console.WriteLine("Removed lobby player at " + count + " ip: " + lobbyplayer.ip.ToString() + " class: " + lobbyplayer.character);
                    break;
                }
                if (isready) //modify otherwise
                {
                    lobbyplayer.isready = isready;
                }
                if (timeunactive == 0)
                {
                    lobbyplayer.timeunactive = 0;
                }
                if (character == "Warrior" || character == "Wizzard" || character == "Bard")
                {
                    lobbyplayer.character = character;
                }
                if (receivedworld)
                {
                    lobbyplayer.receivedworld = true;
                }
            }
            count++;
        }
        if (newplayer)
        {
            playerlist.Add(new LobbyPlayer(ip, isready, ishost, character));
            Console.WriteLine("Added lobbyplayer");
        }
    }

    public bool IsHost(IPAddress ip) //goes over all lobbyplayers until it finds a matching ip, then checks then returns the bool ishost
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

    public override string ToString()
    {
        string message = "";
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            message += "\n" + lobbyplayer.ip.ToString() + ", " + lobbyplayer.isready + ", " + lobbyplayer.ishost + ", " + lobbyplayer.character + ", " + lobbyplayer.timeunactive + ", " + lobbyplayer.receivedworld;
        }
        return message;
        //Playerlist: 
        //196.168.21.4, false, true, bard, 0 false
        //196.168.21.24, false, false, warrior, 0 false
        //196.168.21.3, false, false, warrior, 0 false
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

    public bool IsReady(IPAddress ip)
    {
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            if (lobbyplayer.ip == ip)
            {
                return lobbyplayer.isready;
            }
        }
        return true;
    }

    public bool AllReceivedWorld()
    {
        foreach (LobbyPlayer lobbyplayer in playerlist)
        {
            if (!lobbyplayer.ishost && !lobbyplayer.receivedworld) //is someone is not ready and not host
            {
                return false;
            }
        }
        return true;
    }
}