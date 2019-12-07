using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


enum TileType
{
    Background,
    Floor,
    Wall
}

class Tile : SpriteGameObject
{
    protected TileType type;

    public Tile(string assetname = "", TileType tp = TileType.Background, int layer = 0, string id = "")
        : base (assetname, layer, id, 0)
    {
        type = tp;
    }

    public TileType TileType
    {
        get { return type; }
    }

    public void CheckSprite()
    {
        LevelGrid levelGrid = GameWorld.Find("tiles") as LevelGrid;
        int xFloor = (int)position.X / levelGrid.CellWidth;
        int yFloor = (int)position.Y / levelGrid.CellHeight;

        origin = new Vector2(levelGrid.CellWidth / 2, sprite.Height- levelGrid.CellHeight / 2);

        /*
        bool bottom = levelGrid.GetTileType(xFloor, yFloor + 1) == TileType.Wall;
        bool top = levelGrid.GetTileType(xFloor, yFloor - 1) == TileType.Wall;

        if (top && bottom)
        {
            sprite.SheetIndex = 1;
        }
        else if (top)
        {
            sprite.SheetIndex = 2;
        }
        else if (bottom)
        {
            sprite.SheetIndex = 0;
        }
        */
    }
}

