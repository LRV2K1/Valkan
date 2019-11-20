using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class IO
{
    static void Main(string[] args)
    {        
        ReadFile(); 
        ChangeStats();
        ReadFile();
        Console.ReadLine();        
    }

    static void ReadFile()
    {
        string[] lines;      
        var list = new List<string>();
        FileStream CharacterStats = new FileStream(@"D:\Uni\InputOutput\Stats.txt", FileMode.Open);
        using (var r = new StreamReader(CharacterStats))
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                list.Add(line);
            }
        }
        lines = list.ToArray();
        for (int i = 0; i < lines.Length; i ++)
        {           
            switch (i)
            {
                case 0:
                    Console.WriteLine("Name: " + lines[i]);
                    break;
                case 1:
                    Console.WriteLine("Strength: " + lines[i]);
                    break;
                case 2:
                    Console.WriteLine("Magic " + lines[i]);
                    break;
                case 3:
                    Console.WriteLine("Speed: " + lines[i]);
                    break;
            }
        }
    }

    static void ChangeStats()
    {
        string input;
        input = Console.ReadLine();
        string[] lines;
        var list = new List<string>();
        FileStream CharacterStats = new FileStream(@"D:\Uni\InputOutput\Stats.txt", FileMode.Open);
        using (var r = new StreamReader(CharacterStats))
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                list.Add(line);
            }
        }
        lines = list.ToArray();
        lines[0] = lines[0].Replace(lines[0], input);
        StreamWriter writer = new StreamWriter(File.OpenWrite(@"D:\Uni\InputOutput\Stats.txt"));
        writer.Write(lines[0]);
        writer.Close();
    }
}
