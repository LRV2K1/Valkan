using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class EditorWallTile : Tile
{
    public EditorWallTile(Point grid, string assetname = "", TileType tp = TileType.Wall, TextureType tt = TextureType.None, int layer = 0, string id = "")
        : base(grid, assetname, tp, tt, layer, id)
    {
        tileobject = TileObject.WallTile;
    }

    public override void InitializeTile()
    {
        //set origin
        base.InitializeTile();
        EditorLevelGrid levelGrid = GameWorld.GetObject("levelgrid") as EditorLevelGrid;
        origin = new Vector2(Width / 2, Height - levelGrid.CellHeight / 2 - 1);
    }

    //autotiling algorithm
    public override int CalculateSurroundingStraightTiles()
    {
        EditorLevelGrid levelGrid = GameWorld.GetObject("levelgrid") as EditorLevelGrid;
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

    //autotiling algorithm
    public override int CalculateSurroundingSideTiles()
    {
        EditorLevelGrid levelGrid = GameWorld.GetObject("levelgrid") as EditorLevelGrid;
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

