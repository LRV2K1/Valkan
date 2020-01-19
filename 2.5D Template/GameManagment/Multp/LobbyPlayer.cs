using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

public class LobbyPlayer
{
    public IPAddress ip;
    public bool ishost = false;
    public bool isready = false;
    public int receivedworld = 0;
    public string character = "Warrior";
    public float timeunactive = 0; //if this number reaches 5 the client is clearly no longer there and should be removed (e.g. alt f4)

    public LobbyPlayer(IPAddress ip, bool isready = false, bool ishost = false, string character = "Warrior")
    {
        if (character == "null")
        {
            this.character = "Warrior";
        }
        else
        {
            this.character = character;
        }
        this.ip = ip;
        this.isready = isready;
        this.ishost = ishost;
    }
}