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
        tileobject = TileObject.WallTile;
    }

    
    public override void SetSprite()
    {
        if (sprite.NumberSheetElements < 16)
        {
            sprite.SheetIndex = 0;
        }
        else
        {
            base.SetSprite();
        }
    }
    
    
    public override int CalculateSurroundingTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        int i = 0;
        if (levelGrid.GetTileType(grid.X, grid.Y - 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y - 1) == this.texturetype)
        {
            i += 1;
        }
        if (levelGrid.GetTileType(grid.X + 1, grid.Y) == TileType.Wall && levelGrid.GetTextureType(grid.X + 1, grid.Y) == this.texturetype)
        {
            i += 2;
        }
        if (levelGrid.GetTileType(grid.X, grid.Y + 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y + 1) == this.texturetype)
        {
            i += 4;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y) == TileType.Wall && levelGrid.GetTextureType(grid.X - 1, grid.Y) == this.texturetype)
        {
            i += 8;
        }

        if (levelGrid.GetTileType(grid.X + 1, grid.Y - 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y - 1) == this.texturetype)
        {
            i += 16;
        }
        if (levelGrid.GetTileType(grid.X + 1, grid.Y + 1) == TileType.Wall && levelGrid.GetTextureType(grid.X + 1, grid.Y) == this.texturetype)
        {
            i += 32;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y + 1) == TileType.Wall && levelGrid.GetTextureType(grid.X, grid.Y + 1) == this.texturetype)
        {
            i += 64;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y - 1) == TileType.Wall && levelGrid.GetTextureType(grid.X - 1, grid.Y) == this.texturetype)
        {
            i += 128;
        }
        return i;
    }
    
}

