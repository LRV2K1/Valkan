using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

partial class Enemy : MovingEntity
{
    bool nodata;
    bool die_anim, idle_anim, walking_anim, attack_anim;

    private void LoadEnemyData()
    {
        StreamReader streamReader;
        try
        {
            streamReader = new StreamReader("Content/" + dataloc + "/data.txt");
        }
        catch
        {
            Console.WriteLine("No enemy data found for enemy: " + id);
            Console.WriteLine("location: " + dataloc);
            nodata = true;
            return;
        }

        List<string> data = new List<string>();
        string line = streamReader.ReadLine();
        while (line != "" && line != null)
        {
            data.Add(line);
            line = streamReader.ReadLine();
        }

        for (int i = 0; i < data.Count; i++)
        {
            ProcesData(data[i]);
        }
    }

    private void ProcesData(string line)
    {
        string[] linedata = line.Split('/');
        try
        {
            switch (linedata[0])
            {
                default:
                    break;
                case "idle":
                    LoadAnimations(dataloc, linedata[0], int.Parse(linedata[1]), true, true);
                    PlayAnimation("idle_3");
                    currentAnimation = "A";
                    idle_anim = true;
                    break;
                case "attack":
                    LoadAnimations(dataloc, linedata[0], int.Parse(linedata[1]), true, true);
                    damage = int.Parse(linedata[2]);
                    attack_anim = true;
                    break;
                case "walking":
                    LoadAnimations(dataloc, linedata[0], int.Parse(linedata[1]), true, true);
                    speed = int.Parse(linedata[2]);
                    walking_anim = true;
                    break;
                case "die":
                    LoadAnimations(dataloc, linedata[0], int.Parse(linedata[1]), false);
                    die_anim = true;
                    break;
                case "health":
                    health = int.Parse(linedata[1]);
                    break;
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine("Bad data for: " + linedata[0]);
            return;
        }
        catch (TestSpriteExeption e)
        {
            Console.WriteLine("Sprite not found for: " + linedata[0]);
            switch (linedata[0])
            {
                default:
                    break;
                case "idle":
                    idle_anim = false;
                    break;
                case "attack":
                    attack_anim = false;
                    break;
                case "walking":
                    walking_anim = false;
                    break;
                case "die":
                    die_anim = false;
                    break;
            }
        }
    }

    public bool NoData
    {
        get { return nodata; }
    }
}