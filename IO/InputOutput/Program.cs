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
        string[] lines;
        var list = new List<string>();
        FileStream CharacterStats = new FileStream(FilePath, FileMode.Open);
        using (var r = new StreamReader(CharacterStats))
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                list.Add(line);
            }
        }
        lines = list.ToArray();
        for (int i = 0; i < lines.Length; i++)
        {
            switch (i)
            {
                case 0:
                    Console.WriteLine("Name: " + lines[i]);
                    break;
                case 1:
                    Console.WriteLine("HP: " + lines[i]);
                    break;
                case 2:
                    Console.WriteLine("Strength: " + lines[i]);
                    break;
                case 3:
                    Console.WriteLine("Range: " + lines[i]);
                    break;
                case 4:
                    Console.WriteLine("Speed: " + lines[i]);
                    break;
            }
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
        newHP = 100;
        newStrength = 15;
        newRange = 5;
        newSpeed = 7;
        string[] lines;
        var list = new List<string>();
        FileStream CharacterStats = new FileStream(FilePath, FileMode.Open);
        using (var r = new StreamReader(CharacterStats))
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                list.Add(line);
            }
        }
        lines = list.ToArray();
        StreamWriter writer = new StreamWriter(File.OpenWrite(FilePath));
        lines[0] = lines[0].Replace(lines[0], newName);
        lines[1] = lines[1].Replace(lines[1], Environment.NewLine + newHP);
        lines[2] = lines[2].Replace(lines[2], Environment.NewLine + newStrength);
        lines[3] = lines[3].Replace(lines[3], Environment.NewLine + newRange);
        lines[4] = lines[4].Replace(lines[4], Environment.NewLine + newSpeed);
        writer.Write(lines[0]);
        writer.Write(lines[1]);
        writer.Write(lines[2]);
        writer.Write(lines[3]);
        writer.Write(lines[4]);
        writer.Close();
    }
}
