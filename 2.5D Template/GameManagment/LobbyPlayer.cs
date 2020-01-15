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
    public string character = "null";

    public LobbyPlayer(IPAddress ip, bool ishost = false)
    {
        this.ip = ip;
        this.ishost = ishost;
    }
}
