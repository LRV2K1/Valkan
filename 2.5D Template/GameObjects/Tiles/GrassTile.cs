using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class GrassTile : Tile
{
    public GrassTile(Point grid, string assetname = "", TileType tp = TileType.Background, TextureType tt = TextureType.Grass, int layer = 0, string id = "")
        : base(grid, assetname, tp, tt, layer, id)
    {
        tileobject = TileObject.GrassTile;
    }

    //autotiling algorithm
    public override int CalculateSurroundingStraightTiles()
    {

        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        //regt
        int r = 0;
        if (levelGrid.GetTextureType(grid.X, grid.Y - 1) == TextureType.Water)
        {
            r += 1;
        }
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y) == TextureType.Water)
        {
            r += 2;
        }
        if (levelGrid.GetTextureType(grid.X, grid.Y + 1) == TextureType.Water)
        {
            r += 4;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y) == TextureType.Water)
        {
            r += 8;
        }
        return r;
    }

    public override int CalculateSurroundingSideTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        //schuin
        int s = 0;
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y - 1) == TextureType.Water)
        {
            s += 1;
        }
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y + 1) == TextureType.Water)
        {
            s += 2;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y + 1) == TextureType.Water)
        {
            s += 4;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y - 1) == TextureType.Water)
        {
            s += 8;
        }
        return s;
    }
}