using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class LevelGrid : GameObjectGrid
{
    public LevelGrid(int collumns, int rows, int layer = 0, string id = "")
        : base(collumns, rows, layer, id)
    {

    }

    public override void Add(GameObject obj, int x, int y)
    {
        grid[x, y] = obj;
        obj.Parent = this;
        obj.Position = new Vector2(x * cellWidth/2 - cellWidth/2*y, y * cellHeight/2 + cellHeight/2*x);
    }


    public TileType GetTileType(int x, int y)
    {
        if (x < 0 || x >= Columns)
        {
            return TileType.Wall;
        }
        if (y < 0 || y >= Rows)
        {
            return TileType.Wall;
        }
        Tile current = Objects[x, y] as Tile;
        return current.TileType;
    }



    public Vector2 GridPosition(Vector2 pos)
    {
        return new Vector2(pos.X/cellWidth + pos.Y/cellHeight, -pos.X/CellWidth + pos.Y/cellHeight);
    }
}

