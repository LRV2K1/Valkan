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
        StreamReader streamReader;
        try
        {
            streamReader = new StreamReader(path);
        }
        catch
        {
            Console.WriteLine("level not found for paht: " + path);
            GameEnvironment.OutputWindow("level not found for paht: " + path);
            try
            {
                switch (GameEnvironment.GameSettingsManager.GetValue("connection"))
                {
                    case "offline":
                        GameEnvironment.ScreenFade.TransitionToScene("offlineSelectionState");
                        break;
                    case "online":
                        GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState");
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                GameEnvironment.ScreenFade.TransitionToScene("modeSelectionState");
            }
            return;
        }  

        Dictionary<char, string> tiletypes = ReadTypes(streamReader);
        List<string> tilegrid = ReadGrid(streamReader);
        int width = tilegrid[0].Length;

        LoadTiles(tilegrid, width, tiletypes);

        Dictionary<char, string> entitytypes = ReadTypes(streamReader);
        List<string> entitygrid = ReadGrid(streamReader);
        width = entitygrid[0].Length;

        LoadEntities(entitygrid, width, entitytypes);
    }

    private Dictionary<char, string> ReadTypes(StreamReader streamReader)
    {
        Dictionary<char, string> types = new Dictionary<char, string>();
        string line = streamReader.ReadLine();
        while (line != null && line != "")
        {
            string[] type = line.Split(':');
            char[] a = type[0].ToCharArray();
            types.Add(a[0], type[1]);
            line = streamReader.ReadLine();
        }
        return types;
    }

    private List<string> ReadGrid(StreamReader streamReader)
    {
        List<string> grid = new List<string>();
        string line = streamReader.ReadLine();
        while(line != null && line != "")
        {
            grid.Add(line);
            line = streamReader.ReadLine();
        }
        return grid;
    }
}