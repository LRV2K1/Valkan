using System;
using System.Net;
using System.IO;

public partial class Connection
{
    public string GetReceivedData() //returns string of received data
    {
        return data;
    }
    
    private void HandleReceivedData(string message) //inspect received data and take action
    {
        string[] variables = message.Split(' ');
        if (variables[0] == "Entity:")
        {
            data = message; //store received data locally as a string
        }
        else if (variables[0] == "GetWorld:")
        {
            Send("World " + variables[1] + ":\n" + WorldToString(variables[1]));
        }
        else if (variables[0] == "World") //todo check if we are not having the world
        {
            Console.WriteLine("test");
            StoreWorld(variables[1].TrimEnd(':'), message);
            Console.WriteLine("test2");
        }
        else if (variables[0] == "GetPlayerList:")
        {
            Send("Playerlist: " + PlayerListToString());
        }
        else if (variables[0] == "AddToPlayerList:")
        {
            Send("Playerlist: " + PlayerListToString() + "\n" + variables[1]); //TODO
        }
        // if (variables[0] == "PlayerList:")
        else
        {
            StorePlayerList(message);
        }
    }

    private void StoreWorld(string file, string world) //write to <file>.txt from a single string containing the world
    {
        Console.WriteLine("test24");
        string path = "Content/Levels/" + file + ".txt";
        StreamWriter writer = new StreamWriter(path);
        string[] lines = world.Split('\n');
        Console.WriteLine("File " + file);
        for (int i = 1; i < lines.Length; i++)
        {
            writer.WriteLine(lines[i]);
        }
        Console.WriteLine("Successfully wrote " + (lines.Length - 1) + " lines to " + path);
        writer.Close();
        data = "NICEWORLD";
    }

    private void StorePlayerList(string list) //store string of ips to array of IPs
    {
        string[] lines = list.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            //playerlist[i] = IPAddress.Parse(lines[i]);
            playerlist.Clear();
            playerlist.Add(IPAddress.Parse(lines[i]));
        }
        Console.WriteLine("Successfully stored " + (lines.Length - 1) + " players's IPs");
        data = "NICE";
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
                message += line + "\n";
                line = streamReader.ReadLine();
            }
        }
        streamReader.Close();
        return message;
    }

    public string PlayerListToString()
    {
        string message = "";
        for (int i = 0; i < playerlist.Count; i++)
        {
            message += "\n" + playerlist[i];
        }
        return message; ;
    }
}
