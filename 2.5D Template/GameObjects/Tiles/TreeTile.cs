using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        if (boundingbox == Rectangle.Empty && TileType == TileType.Wall)
        {
            boundingbox = new Rectangle((int)(GlobalPosition.X - levelGrid.CellWidth / 2), (int)(GlobalPosition.Y - levelGrid.CellHeight / 2), levelGrid.CellWidth, levelGrid.CellHeight);
        }
    }
}

