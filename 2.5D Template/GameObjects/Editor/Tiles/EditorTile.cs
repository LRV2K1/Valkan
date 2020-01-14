using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class test : SpriteGameObject
{

    protected TileType type;
    protected TextureType texturetype;
    protected TileObject tileobject;
    protected Point grid;

    public test(Point grid, string assetname = "", TileType tp = TileType.Background, TextureType tt = TextureType.None, int layer = 0, string id = "")
        : base(assetname, layer, id, 0)
    {
        tileobject = TileObject.Tile;
        this.grid = grid;
        texturetype = tt;
        type = tp;
    }

    public override void Reset()
    {
        InitializeTile();
    }

    //change tile
    public virtual void ChangeTile(TileType tp, TextureType tt, string assetName = "")
    {
        sprite = null;
        sprite = new SpriteSheet(assetName);
        texturetype = tt;
        type = tp;

        //update surrounding tiles
        EditorLevelGrid levelGrid = GameWorld.GetObject("levelgrid") as EditorLevelGrid;
        for (int x = grid.X -1; x <= grid.X + 1; x++)
        {
            for (int y = grid.Y - 1; y <= grid.Y + 1; y++)
            {
                Tile tile = levelGrid.Get(x, y) as Tile;
                if (tile != null)
                {
                    tile.InitializeTile();
                }
            }
        }
    }

    public TileType TileType
    {
        get { return type; }
    }

    public TextureType TextureType
    {
        get { return texturetype; }
    }

    public TileObject TileObject
    {
        get { return tileobject; }
    }

    public virtual void InitializeTile()
    {
        //set origin and sprite
        if (type == TileType.Background)
        {
            return;
        }

        origin = new Vector2(Width/2, sprite.Height / 2);

        SetSprite();
    }

    //set sprite
    public virtual void SetSprite()
    {
        //autotiling algorithm
        int r = CalculateSurroundingStraightTiles();
        int s = CalculateSurroundingSideTiles();
        if (r != 0)
        {
            sprite.SheetIndex = r % 16;
        }
        else
        {
            sprite.SheetIndex = s % 16 + 16;
        }
    }

    //autotiling algorithm
    public virtual int CalculateSurroundingStraightTiles()
    {

        EditorLevelGrid levelGrid = GameWorld.GetObject("levelgrid") as EditorLevelGrid;
        //regt
        int r = 0;
        if (levelGrid.GetTextureType(grid.X, grid.Y - 1) == texturetype)
        {
            r += 1;
        }
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y) == texturetype)
        {
            r += 2;
        }
        if (levelGrid.GetTextureType(grid.X, grid.Y + 1) == texturetype)
        {
            r += 4;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y) == texturetype)
        {
            r += 8;
        }
        return r;
    }

    //autotiling algorithm
    public virtual int CalculateSurroundingSideTiles()
    {
        EditorLevelGrid levelGrid = GameWorld.GetObject("levelgrid") as EditorLevelGrid;
        //schuin
        int s = 0;
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y - 1) == texturetype)
        {
            s += 1;
        }
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y + 1) == texturetype)
        {
            s += 2;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y + 1) == texturetype)
        {
            s += 4;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y - 1) == texturetype)
        {
            s += 8;
        }
        return s;
    }

}

