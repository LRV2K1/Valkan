using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class TreeTile : Tile
{
    public TreeTile(Point grid, string assetname = "", TileType tp = TileType.Wall, TextureType tt = TextureType.None, int layer = 0, string id = "")
    : base(grid, assetname.ToString(), tp, tt, layer, id)
    {
        tileobject = TileObject.TreeTile;
    }

    public override void InitializeTile()
    {
        base.InitializeTile();

        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;

        origin = new Vector2(Width / 2, sprite.Height - levelGrid.CellHeight / 2);
    }
}