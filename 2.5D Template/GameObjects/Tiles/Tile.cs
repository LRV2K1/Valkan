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

partial class Tile : SpriteGameObject
{

    protected TileType type;
    protected TextureType texturetype;
    protected TileObject tileobject;
    protected Rectangle boundingbox;
    protected List<string> drawPassengers;
    protected List<string> passengers;
    protected Point grid;
    protected bool hasWalls;
    protected int hasItem;

    public Tile(Point grid, string assetname = "", TileType tp = TileType.Background, TextureType tt = TextureType.None, int layer = 0, string id = "")
        : base(assetname, layer, id, 0)
    {
        tileobject = TileObject.Tile;
        this.grid = grid;
        texturetype = tt;
        type = tp;
        drawPassengers = new List<string>();
        passengers = new List<string>();
        hasItem = 0;
    }

    public override void Reset()
    {
        base.Reset();
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
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        for (int x = grid.X - 1; x <= grid.X + 1; x++)
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

    public void AddPassenger(string id)
    {
        if (GameWorld.GetObject(id) is Item)
        {
            hasItem++;
        }
        passengers.Add(id);
    }

    public void RemovePassenger(string id)
    {
        if (GameWorld.GetObject(id) is Item)
        {
            hasItem--;
        }
        passengers.Remove(id);
    }

    //add passenger
    public void AddDrawPassenger(GameObject obj)
    {
        for (int i = 0; i < drawPassengers.Count; i++)
        {
            if (GameWorld.GetObject(drawPassengers[i]).Position.Y > obj.Position.Y)
            {
                drawPassengers.Insert(i, obj.Id);
                return;
            }
        }
        drawPassengers.Add(obj.Id);
    }

    //remove passenger
    public void RemoveDrawPassenger(string id)
    {
        drawPassengers.Remove(id);
    }

    //passenger order
    public void CheckDrawPassengerPosition(GameObject obj)
    {
        for (int i = 0; i < drawPassengers.Count; i++)
        {
            if (drawPassengers[i] == obj.Id)
            {
                if (i != 0)
                {
                    if (GameWorld.GetObject(drawPassengers[i - 1]).Position.Y > obj.Position.Y)
                    {
                        RemoveDrawPassenger(obj.Id);
                        AddDrawPassenger(obj);
                        return;
                    }
                }

                if (i < drawPassengers.Count - 1)
                {
                    if (GameWorld.GetObject(drawPassengers[i + 1]).Position.Y < obj.Position.Y)
                    {
                        RemoveDrawPassenger(obj.Id);
                        AddDrawPassenger(obj);
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

        origin = new Vector2(Width / 2, sprite.Height / 2);
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        if (levelGrid != null)
        {
            if (boundingbox == Rectangle.Empty && TileType == TileType.Wall)
            {
                boundingbox = new Rectangle((int)(GlobalPosition.X - levelGrid.CellWidth / 2), (int)(GlobalPosition.Y - levelGrid.CellHeight / 2), levelGrid.CellWidth, levelGrid.CellHeight);
            }
        }

        SetSprite();
        CheckHasWalls();
    }

    //sets sprite
    public virtual void SetSprite()
    {
        //autotiling alogrithm
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

    private void CheckHasWalls()
    {
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        if (levelGrid.GetTileType(grid.X, grid.Y - 1) == TileType.Wall)
        {
            hasWalls = true;
            return;
        }
        if (levelGrid.GetTileType(grid.X + 1, grid.Y) == TileType.Wall)
        {
            hasWalls = true;
            return;
        }
        if (levelGrid.GetTileType(grid.X, grid.Y + 1) == TileType.Wall)
        {
            hasWalls = true;
            return;
        }
        if (levelGrid.GetTileType(grid.X - 1, grid.Y) == TileType.Wall)
        {
            hasWalls = true;
            return;
        }
        hasWalls = false;
    }

    //autotiling algorithm
    public virtual int CalculateSurroundingStraightTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
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

    //autotiling alogrithm
    public virtual int CalculateSurroundingSideTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
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

    public List<string> DrawPassengers
    {
        get { return drawPassengers; }
        set { drawPassengers = value; }
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

    public bool HasWalls
    {
        get { return hasWalls; }
    }

    public bool HasItem
    {
        get { return hasItem > 0; }
    }
}

