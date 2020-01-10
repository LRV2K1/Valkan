using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

partial class Enemy
{
    bool nodata;

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

        LoadAnimation(dataloc + "/spr_idle_0@4", "sprite", true, true);
        LoadAnimation(dataloc + "/spr_die_0@8", "die", false, false);
        PlayAnimation("sprite");

    }

    public bool NoData
    {
        get { return nodata; }
    }
}
