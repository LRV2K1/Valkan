using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


enum TileType
{
    Background,
    Floor,
    Wall
}

enum TextureType
{
    None,
    Grass,
    Water,
    Mine
}

enum TileObject
{
    Tile,
    GrassTile,
    WallTile,
    TreeTile
}

class Tile : SpriteGameObject
{
    protected TileType type;
    protected TextureType texturetype;
    protected TileObject tileobject;
    protected Rectangle boundingbox;
    protected List<string> passengers;
    protected Point grid;

    public Tile(string assetname = "", TileType tp = TileType.Background, int layer = 0, string id = "")
        : base (assetname, layer, id, 0)
    {
        tileobject = TileObject.Tile;
        this.grid = grid;
        texturetype = tt;
        type = tp;
        passengers = new List<string>();
    }

    public override void Reset()
    {
        base.Reset();
        InitializeTile();
    }

    //add passenger
    public void AddPassenger(GameObject obj)
    {
        for (int i = 0; i < passengers.Count; i++)
        {
            if (GameWorld.GetObject(passengers[i]).Position.Y > obj.Position.Y)
            {
                passengers.Insert(i, obj.Id);
                return;
            }
        }
        passengers.Add(obj.Id);
    }

    //remove passenger
    public void RemovePassenger(string id)
    {
        for (int i = 0; i < Passengers.Count; i++)
        {
            if (passengers[i] == id)
            {
                passengers.RemoveAt(i);
                break;
            }
        }
    }

    //passenger order
    public void CheckPassengerPosition(GameObject obj)
    {
        for (int i = 0; i < passengers.Count; i++)
        {
            if (passengers[i] == obj.Id)
            {
                if (i != 0)
                {
                    if (GameWorld.GetObject(passengers[i -1]).Position.Y > obj.Position.Y)
                    {
                        RemovePassenger(obj.Id);
                        AddPassenger(obj);
                        return;
                    }
                }

                if (i < passengers.Count - 1)
                {
                    if (GameWorld.GetObject(passengers[i + 1]).Position.Y < obj.Position.Y)
                    {
                        RemovePassenger(obj.Id);
                        AddPassenger(obj);
                        return;
                    }
                }
                return;
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

    //set tile
    public virtual void InitializeTile()
    {
        if (type == TileType.Background)
        {
            return;
        }

        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;

        origin = new Vector2(Width / 2, sprite.Height / 2);

        if (boundingbox == Rectangle.Empty && TileType == TileType.Wall)
        {
            boundingbox = new Rectangle((int)(GlobalPosition.X - levelGrid.CellWidth/2), (int)(GlobalPosition.Y - levelGrid.CellHeight / 2), levelGrid.CellWidth, levelGrid.CellHeight);
        }

        SetSprite();
    }

    //sets sprite
    public virtual void SetSprite()
    {
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
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
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


    public virtual int CalculateSurroundingSideTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
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

    public List<string> Passengers
    {
        get { return passengers; }
        set { passengers = value; }
    }

    public Rectangle GetBoundingBox()
    { return boundingbox; }

    public void SetBoundingBox(Rectangle value)
    { boundingbox = value; }
}

