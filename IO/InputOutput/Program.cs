using System;
using System.IO;
using System.Collections.Generic;

class IO
{
    public static string FilePath = @"\Users\leung\Documents\GitHub\Valkan\IO\InputOutput\PlayerStats\Stats.txt";   
    static void Main(string[] args)
    {
        ChangeStats();
        ReadFile();
        Console.ReadLine();
    }

    static void ReadFile()
    {
        StreamReader streamReader = new StreamReader(FilePath);
        List<string> lines = new List<string>();
        string line = streamReader.ReadLine();
        while(line != null)
        {
            lines.Add(line);
            line = streamReader.ReadLine();
        }
        for (int i = 0; i < lines.Count; i++)
        {
            Console.WriteLine(lines[i]);
        }
    }

    static void ChangeStats()
    {
        string newName;
        int newHP;
        int newStrength;
        int newRange;
        int newSpeed;
        newName = "Valkan";
        newHP = 10;
        newStrength = 1;
        newRange = 5;
        newSpeed = 7;
        string[] lines;
        List<string> list = new List<string>();
        lines = new string[5];
        StreamWriter writer = new StreamWriter(FilePath);

        lines[0] = newName.ToString();
        lines[1] = newHP.ToString();
        lines[2] = newStrength.ToString();
        lines[3] = newRange.ToString();
        lines[4] = newSpeed.ToString();
        
        for (int i = 0; i < lines.Length; i++)
        {
            writer.WriteLine(lines[i]);
        }
        writer.Close();
    }
}
