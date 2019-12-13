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

    //set tile
    public override void InitializeTile()
    {
        base.InitializeTile();

        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;

        origin = new Vector2(Width / 2, sprite.Height - levelGrid.CellHeight / 2 - 1);

        SetBoundingBox();
    }

    //set sprite
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

    //set boundingbox
    private void SetBoundingBox()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        int i = sprite.SheetIndex;
        if ((i%3 == 0 && i < 16) || i == 23 || i == 27 || i == 29 || i == 30)
        {
            if ( i == 3 || i == 27)
            {
                boundingbox.Width /= 2;
            }
            else if (i == 6 || i == 23)
            {
                boundingbox.Height /= 2;
            }
            else if (i == 9 || i == 29)
            {
                boundingbox.Height /= 2;
                boundingbox.Y += boundingbox.Height;
            }
            else if (i == 12 || i == 30)
            {
                boundingbox.Width /= 2;
                boundingbox.X += boundingbox.Width;
            }
        }
    }
    //autotiling alogrithm
    public override int CalculateSurroundingStraightTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        int i = 0;

        if (levelGrid.GetTileObject(grid.X, grid.Y - 1) != TileObject.WallTile || levelGrid.GetTextureType(grid.X, grid.Y - 1) != this.texturetype)
        {
            i += 1;
        }
        if (levelGrid.GetTileObject(grid.X + 1, grid.Y) != TileObject.WallTile || levelGrid.GetTextureType(grid.X + 1, grid.Y) != this.texturetype)
        {
            i += 2;
        }
        if (levelGrid.GetTileObject(grid.X, grid.Y + 1) != TileObject.WallTile || levelGrid.GetTextureType(grid.X, grid.Y + 1) != this.texturetype)
        {
            i += 4;
        }
        if (levelGrid.GetTileObject(grid.X - 1, grid.Y) != TileObject.WallTile || levelGrid.GetTextureType(grid.X - 1, grid.Y) != this.texturetype)
        {
            i += 8;
        }
        return i;

    }

    public override int CalculateSurroundingSideTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        int i = 0;

        if (levelGrid.GetTileType(grid.X + 1, grid.Y - 1) != TileType.Wall || levelGrid.GetTextureType(grid.X, grid.Y - 1) != this.texturetype)
        {
            i += 1;
        }
        if (levelGrid.GetTileType(grid.X + 1, grid.Y + 1) != TileType.Wall || levelGrid.GetTextureType(grid.X + 1, grid.Y) != this.texturetype)
        {
            i += 2;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y + 1) != TileType.Wall || levelGrid.GetTextureType(grid.X, grid.Y + 1) != this.texturetype)
        {
            i += 4;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y - 1) != TileType.Wall || levelGrid.GetTextureType(grid.X - 1, grid.Y) != this.texturetype)
        {
            i += 8;
        }
        return i;
    }

}

