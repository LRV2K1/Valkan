using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using System.IO;
using System.Net;
using System.Text;

public class ConnectionParty : Connection
{
    public PlayerList playerlist;
    float time;
    public bool isopen = true;

    public ConnectionParty(int port)
        : base(port)
    {
        playerlist = new PlayerList();
        ar_ = udpclient.BeginReceive(Receive, new object());
        Console.WriteLine("Created party connection");
    }

    private void Receive(IAsyncResult ar)
    {
        try
        {
            byte[] bytes = udpclient.EndReceive(ar, ref remoteep); //store received data in byte array

            //if (ip.Address.ToString() != MyIP().ToString()) //check if we did not receive from local ip (we dont need our own data) 
            //{
            string message = Encoding.ASCII.GetString(bytes); //convert byte array to string
            Console.WriteLine("\nReceivedd from {1}" + port + " ->\n{0}", message, remoteep.Address.ToString());
            HandleReceivedData(message);
            //}
            ar_ = udpclient.BeginReceive(Receive, new object()); ; //repeat
        }
        catch
        {

        }
    }
    public void Update(GameTime gameTime) //manage unexpected disconnect
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (playerlist.IsHost(MyIP()) && time > 1)
        {
            if (playerlist.playerlist.Count > 1) //do only if host is not alone in a party
            {
                Send("Host is still connected", port, false); //message send by host only, if host crashes the clients wont be stuck
                playerlist.Modify(MyIP(), timeunactive: 0);
                foreach (LobbyPlayer lobbyplayer in playerlist.playerlist) //remove unactive players
                {
                    lobbyplayer.timeunactive += 1;
                    if (lobbyplayer.timeunactive >= 5)
                    {
                        playerlist.Modify(lobbyplayer.ip, false, false, true); //remove 1 player from the party
                        Send("Playerlist:" + playerlist.ToString(), port, false);
                        isopen = true; //since 1 player has been kicked, there is space
                        break;
                    }
                }
            }

            if (GameEnvironment.GameStateManager.CurrentGameState.ToString() != "PlayingState" && isopen)
            {
                Send("Playerlist " + port + " :" + playerlist.ToString(), 9967, true); //broadcast playerlist to port 1000
            }
            time = 0;
        }
        else
        {
            try
            {
                if (time > 1 && playerlist.playerlist[1].ip.ToString() == MyIP().ToString()) //for player2 only
                {
                    Send("I am still connected", port, false); //message send by clients, this prevents error when a client types alt + f4.
                    playerlist.Modify(MyIP(), timeunactive: 0);
                    foreach (LobbyPlayer lobbyplayer in playerlist.playerlist)
                    {
                        if (lobbyplayer.ishost)
                        {
                            lobbyplayer.timeunactive += 1;
                            if (lobbyplayer.timeunactive >= 5)
                            {
                                Disconnect();
                            }
                        }
                    }
                    time = 0;
                }
            }
            catch
            {

            }
        }
    }

    public void HandleReceivedData(string message) //inspect received data and take action
    {
        string[] lines = message.Split('\n');
        string[] variables = message.Split(' ');
        bool log = true;
        if (playerlist.IsHost(MyIP())) //data for host only
        {
            if (variables[0] == "Entity:")
            {
                log = false;
            }
            else if (message == "Join")
            {
                //playerlist.Modify(sender);
                Send("Playerlist:" + playerlist.ToString(), port);
                if (playerlist.playerlist.Count > 3) //if the party has 4 members close it
                {
                    Send("Closed: " + MyIP().ToString() + ":" + port, 9967);
                    isopen = false;
                }
            }
            else if (message == "Leave")
            {
               // playerlist.Modify(sender, false, false, true);
                Send("Playerlist:" + playerlist.ToString(), port);
                isopen = true;
            }
            else if (message == "Ready")
            {
                //playerlist.Modify(sender, true);
                Send("Playerlist:" + playerlist.ToString(), port);
            }
            else if (variables[0] == "Character:")
            {
               // playerlist.Modify(sender, character: variables[1]);
                Send("Playerlist:" + playerlist.ToString(), port);
            }
            else if (lines[0] == "Playerlist:")
            {
                playerlist.Store(message);
                if (GameEnvironment.GameStateManager.CurrentGameState.ToString() == "PlayingState")
                {
                    log = false;
                }
            }
            else if (message == "I am still connected")
            {
              //  playerlist.Modify(sender, timeunactive: 0);
                log = false;
            }
            else
            {
                Console.WriteLine("ERROR! The message:\n" + message + "\nis not a valid message");
            }
        }
        else //data for everyone but host
        {
            if (variables[0] == "Entity:")
            {
                log = false;
            }
            else if (lines[0] == "Playerlist:")
            {
                playerlist.Store(message);
                
                if (GameEnvironment.GameStateManager.CurrentGameState.ToString() == "PlayingState")
                {
                    log = false;
                }
            }
            else if (message == "Start")
            {
                GameEnvironment.ScreenFade.TransitionToScene("playingState");
            }
            else if (message == "HostLeaves") //if the host leaves then disconnect and go back to portselectionstate
            {
                Disconnect();
            }
            else if (message == "Host is still connected")
            {
                //playerlist.Modify(sender, timeunactive: 0);
                if (playerlist.playerlist[1].ip.ToString() == MyIP().ToString()) //send only by player2
                {
                    Send("Playerlist:" + playerlist.ToString(), port, false);
                    log = false;
                }
            }
            else if (variables[0] == "World")
            {
                Console.WriteLine("Received a world");
                StoreWorld(variables[1], message);
            }
            else
            {
                Console.WriteLine("ERROR! The message:\n" + message + "\nis not a valid message");
            }
        }
        if (log) //should the received data be put in console?
        {
            Console.WriteLine("\nReceived from {1}:" + port + " ->\n{0}", message, remoteep, port);
        }
    }

    public void Disconnect() //stop receiving and sending data
    {
        if (playerlist.IsHost(MyIP()))
        {
            Send("HostLeaves", 9999);
            Send("Closed: " + MyIP().ToString() + ":" + port, 9967);
            isopen = false;
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
        else
        {
            Send("Leave", 9999);
            MultiplayerManager.Connect(9967);
            GameEnvironment.ScreenFade.TransitionToScene("portSelectionState", 5);
        }
        udpclient.Close();
        Console.WriteLine("Disconnect from party");
        MultiplayerManager.party = null;
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
        GameEnvironment.GameSettingsManager.SetValue("level", file.Substring(6, file.Length - 6)); //remove Level_ from Level_(number) so we only have the int
        playerlist.Modify(MyIP(), receivedworld: true);
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
                message += "\n" + line;
                line = streamReader.ReadLine();
            }
        }
        streamReader.Close();
        return message;
    }
}