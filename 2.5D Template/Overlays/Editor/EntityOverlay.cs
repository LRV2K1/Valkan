﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

class EntityOverlay : GameObjectList
{
    public EntityOverlay(GameObjectLibrary gameworld, string path, int layer = 101, string id = "")
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
            Button button = MakeButton(x, line);
            if (button == null)
            {
                continue;
            }
            if (button.Height > 230)
            {
                button.Sprite.Size = new Vector2(0.5f, 0.5f);
            }
            x += (int)(button.Width * button.Sprite.Size.X + 10);
            line = streamReader.ReadLine();
        }
        streamReader.Close();
    }

    private Button MakeButton(int x, string line)
    {
        string[] type = line.Split(',');

        if (type.Length < 3)
        {
            return null;
        }

        string asset = type[0];
        int boundingy = int.Parse(type[1]);
        EntityType et = (EntityType) Enum.Parse(typeof(EntityType), type[2]);

        EntityButton button = new EntityButton(asset, boundingy, et);
        if (et == EntityType.Enemy)
        {
            button.EnemyType = (EnemyType)Enum.Parse(typeof(EnemyType), type[3]);
        }
        else if (et == EntityType.AnimatedItem || et == EntityType.SpriteItem)
        {
            button.ItemType = (ItemType)Enum.Parse(typeof(ItemType), type[3]);
        }

        button.Position = new Vector2(x, 230);
        button.Origin += new Vector2(0, button.Height);
        Add(button);
        return button;
    }

    public override bool Visible
    {
        get { return visible; }
        set
        {
            visible = value;
            foreach (string id in children)
            {
                GameWorld.GetObject(id).Visible = visible;
            }
        }
    }
}