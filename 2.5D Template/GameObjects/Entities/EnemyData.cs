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
            string[] currentline = data[i].Split('/');
            try
            {
                switch (currentline[0])
                {
                    default:
                        break;
                    case "idle":
                        LoadAnimations(dataloc, currentline[0], int.Parse(currentline[1]), true, true);
                        PlayAnimation("idle_3");
                        currentAnimation = "A";
                        idle_anim = true;
                        break;
                    case "attack":
                        LoadAnimations(dataloc, currentline[0], int.Parse(currentline[1]), true, true);
                        damage = int.Parse(currentline[2]);
                        attack_anim = true;
                        break;
                    case "walking":
                        LoadAnimations(dataloc, currentline[0], int.Parse(currentline[1]), true, true);
                        speed = int.Parse(currentline[2]);
                        walking_anim = true;
                        break;
                    case "die":
                        LoadAnimations(dataloc, currentline[0], int.Parse(currentline[1]), false);
                        die_anim = true;
                        break;
                    case "health":
                        health = int.Parse(currentline[1]);
                        break;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Bad data for: " + currentline[0]);
                continue;
            }
            catch(TestSpriteExeption e)
            {
                Console.WriteLine("Sprite not found for: " + currentline[0]);
                switch (currentline[0])
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
    }

    public bool NoData
    {
        get { return nodata; }
    }
}