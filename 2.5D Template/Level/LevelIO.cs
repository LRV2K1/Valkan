using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

partial class Level : GameObjectLibrary
{
    //loads all data from the level file
    private void LoadFile(string path)
    {
        Dictionary<char, string> tiletypeschar = new Dictionary<char, string>();
        List<string> textLines = new List<string>();
        StreamReader streamReader = new StreamReader(path);

        //read different tiletypes
        string line = streamReader.ReadLine();
        while (line != "")
        {
            string[] types = line.Split(':');
            char[] a = types[0].ToCharArray();
            tiletypeschar.Add(a[0], types[1]);
            line = streamReader.ReadLine();
        }

        //read tile grid
        line = streamReader.ReadLine();
        int width = line.Length;
        while (line != "" && line != null)
        {
            textLines.Add(line);
            line = streamReader.ReadLine();
        }

        LoadTiles(textLines, width, tiletypeschar);

        if (line == null)
        {
            streamReader.Close();
            return;
        }

        Dictionary<char, string> entitytypeschar = new Dictionary<char, string>();
        List<string> entityLines = new List<string>();

        //read different entitytypes
        line = streamReader.ReadLine();
        while (line != "" && line != null)
        {
            string[] types = line.Split(':');
            char[] a = types[0].ToCharArray();
            entitytypeschar.Add(a[0], types[1]);
            line = streamReader.ReadLine();
        }

        //read entitygrid
        line = streamReader.ReadLine();
        width = line.Length;
        while (line != "" && line != null)
        {
            entityLines.Add(line);
            line = streamReader.ReadLine();
        }

        LoadEntities(entityLines, width, entitytypeschar);

        streamReader.Close();
    }
}
