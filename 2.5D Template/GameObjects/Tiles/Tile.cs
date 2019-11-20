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
}

class Tile : SpriteGameObject
{

    protected TileType type;
    protected TextureType texturetype;
    protected Rectangle boundingbox;
    protected List<string> passengers;
    protected Point grid;

    public Tile(Point grid, string assetname = "", TileType tp = TileType.Background, TextureType tt = TextureType.None, int layer = 0, string id = "")
        : base (assetname, layer, id, 0)
    {
        this.grid = grid;
        texturetype = tt;
        type = tp;
        passengers = new List<string>();
    }

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

    public void InitializeTile()
    {
        if (type == TileType.Background)
        {
            return;
        }

        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;

        origin = new Vector2(Width/2, sprite.Height- levelGrid.CellHeight / 2);
        if (boundingbox == Rectangle.Empty && TileType == TileType.Wall)
        {
            boundingbox = new Rectangle((int)(GlobalPosition.X - levelGrid.CellWidth/2), (int)(GlobalPosition.Y - levelGrid.CellHeight / 2), levelGrid.CellWidth, levelGrid.CellHeight);
        }

        SetSprite();
    }

    public virtual void SetSprite()
    {
        sprite.SheetIndex = CalculateSurroundingTiles()%16;
    }

    public virtual int CalculateSurroundingTiles()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        int i = 0;
        if (levelGrid.GetTextureType(grid.X, grid.Y - 1) != TextureType.None && levelGrid.GetTextureType(grid.X, grid.Y - 1) != texturetype)
        {
            i += 1;
        }
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y) != TextureType.None && levelGrid.GetTextureType(grid.X + 1, grid.Y) != texturetype)
        {
            i += 2;
        }
        if (levelGrid.GetTextureType(grid.X, grid.Y + 1) != TextureType.None && levelGrid.GetTextureType(grid.X, grid.Y + 1) != texturetype)
        {
            i += 4;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y) != TextureType.None && levelGrid.GetTextureType(grid.X - 1, grid.Y) != texturetype)
        {
            i += 8;
        }

        if (levelGrid.GetTextureType(grid.X + 1, grid.Y - 1) != TextureType.None && levelGrid.GetTextureType(grid.X, grid.Y - 1) != texturetype)
        {
            i += 16;
        }
        if (levelGrid.GetTextureType(grid.X + 1, grid.Y + 1) != TextureType.None && levelGrid.GetTextureType(grid.X + 1, grid.Y) != texturetype)
        {
            i += 32;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y + 1) != TextureType.None && levelGrid.GetTextureType(grid.X, grid.Y + 1) != texturetype)
        {
            i += 64;
        }
        if (levelGrid.GetTextureType(grid.X - 1, grid.Y - 1) != TextureType.None && levelGrid.GetTextureType(grid.X - 1, grid.Y) != texturetype)
        {
            i += 128;
        }
        return i;
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

