using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class TileButton : Button
{
    //button to select tiles
    protected TileType tiletype;
    protected TextureType texturetype;
    protected TileObject tileobject;
    protected string asset;
    public TileButton (string assetname, TileType tp, TextureType tt, TileObject to)
        : base(assetname)
    {
        sprite.SheetIndex = 0;
        asset = assetname;
        tiletype = tp;
        texturetype = tt;
        tileobject = to;
        if (to == TileObject.WallTile)
        {
            sprite.SheetIndex = 6;
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        //give information to the mouse
        base.HandleInput(inputHelper);
        if (pressed)
        {
            EditorMouse mouse = GameWorld.GetObject("mouse") as EditorMouse;
            mouse.Asset = asset;
            mouse.TileType = tiletype;
            mouse.TextureType = texturetype;
            mouse.TileObject = tileobject;
            mouse.Tile = true;
        }
    }
}