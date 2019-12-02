using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

partial class Level : GameObjectLibrary
{
    private void LoadFile(string path)
    {
        Dictionary<char, string> tiletypeschar = new Dictionary<char, string>();
        List<string> textLines = new List<string>();
        StreamReader streamReader = new StreamReader(path);

        string line = streamReader.ReadLine();
        while (line != "")
        {
            string[] types = line.Split(':');
            char[] a = types[0].ToCharArray();
            tiletypeschar.Add(a[0], types[1]);
            line = streamReader.ReadLine();
        }

        line = streamReader.ReadLine();
        int width = line.Length;
        while (line != "")
        {
            textLines.Add(line);
            line = streamReader.ReadLine();
        }

        LoadTiles(textLines, width, tiletypeschar);

        Dictionary<char, string> entitytypeschar = new Dictionary<char, string>();
        List<string> entityLines = new List<string>();

        line = streamReader.ReadLine();
        while (line != "")
        {
            string[] types = line.Split(':');
            char[] a = types[0].ToCharArray();
            entitytypeschar.Add(a[0], types[1]);
            line = streamReader.ReadLine();
        }

        line = streamReader.ReadLine();
        width = line.Length;
        while (line != null)
        {
            entityLines.Add(line);
            line = streamReader.ReadLine();
        }

        LoadEntities(entityLines, width, entitytypeschar);

        streamReader.Close();
    }
}
