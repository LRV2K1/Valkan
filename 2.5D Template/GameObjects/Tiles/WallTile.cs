using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class WallTile : Tile
{
    public WallTile(Point grid, string assetname = "", TileType tp = TileType.Wall, TextureType tt = TextureType.None, int layer = 0, string id = "")
        : base(grid, assetname, tp, tt, layer, id)
    {

    }

    
    public override void SetSprite()
    {
        sprite.SheetIndex = 0;
    }
    

    public override int CalculateSurroundingTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        int i = 0;
        if (levelGrid.GetTileType(grid.X, grid.Y - 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y - 1) == TextureType.None)
        {
            i += 1;
        }
        if (levelGrid.GetTileType(grid.X + 1, grid.Y) == TileType.Wall && levelGrid.GetTextureType(grid.X + 1, grid.Y) == TextureType.None)
        {
            i += 2;
        }
        if (levelGrid.GetTileType(grid.X, grid.Y + 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y + 1) == TextureType.None)
        {
            i += 4;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y) == TileType.Wall && levelGrid.GetTextureType(grid.X - 1, grid.Y) == TextureType.None)
        {
            i += 8;
        }

        if (levelGrid.GetTileType(grid.X + 1, grid.Y - 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y - 1) == TextureType.None)
        {
            i += 16;
        }
        if (levelGrid.GetTileType(grid.X + 1, grid.Y + 1) == TileType.Wall && levelGrid.GetTextureType(grid.X + 1, grid.Y) == TextureType.None)
        {
            i += 32;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y + 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y + 1) == TextureType.None)
        {
            i += 64;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y - 1) == TileType.Wall && levelGrid.GetTextureType(grid.X - 1, grid.Y) == TextureType.None)
        {
            i += 128;
        }
        return i;
    }
}

