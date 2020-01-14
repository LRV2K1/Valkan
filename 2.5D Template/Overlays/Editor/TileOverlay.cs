using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

class TileOverlay : GameObjectList
{
    public TileOverlay(GameObjectLibrary gameworld, string path ,int layer = 101, string id = "")
        : base(layer, id)
    {
        GameWorld = gameworld;
        ReadFile(path);
    }

    private void ReadFile(string path)
    {
        StreamReader streamReader;
        try
        {
            streamReader = new StreamReader(path);
        }
        catch
        {
            return;
        }
        string line = streamReader.ReadLine();
        int x = 20;
        while (line != "" && line != null)
        {
            MakeButton(x, line);
            x += 130;
            line = streamReader.ReadLine();
        }
        streamReader.Close();
    }

    private void MakeButton(int x, string line)
    {
        string[] type = line.Split(',');

        if (type.Length != 4)
        {
            return;
        }

        string asset = type[0];
        TileType tp = (TileType) Enum.Parse(typeof(TileType), type[1]);
        TextureType tt = (TextureType)Enum.Parse(typeof(TextureType), type[2]);
        TileObject to = (TileObject)Enum.Parse(typeof(TileObject), type[3]);

        TileButton button = new TileButton(asset, tp, tt, to);
        button.Position = new Vector2(x, 10);
        Add(button);
    }

    public override bool Visible
    {
        get { return visible; }
        set { 
            visible = value; 
            foreach (string id in children)
            {
                GameWorld.GetObject(id).Visible = visible;
            }
        }
    }
}