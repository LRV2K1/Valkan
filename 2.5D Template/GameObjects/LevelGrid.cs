using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class LevelGrid : GameObjectGrid
{
    public LevelGrid(int collumns, int rows, int layer = 0, string id = "")
        : base(collumns, rows, layer, id)
    {
    }

    public void SetupGrid()
    {
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                //Tile tile = new GrassTile(new Point(x, y), "Sprites/Tiles/spr_cave_sheet_0@4x8", TileType.Floor, TextureType.Grass);
                Tile tile = new GrassTile(new Point(x, y), "Sprites/Tiles/spr_grass_sheet_0@4x8", TileType.Floor, TextureType.Grass);
                Add(tile, x, y);
            }
        }
    }

    public override void Add(GameObject obj, int x, int y)
    {
        GameWorld.Add(obj);
        grid[x, y] = obj.Id;
        obj.Parent = this;
        obj.Position = AnchorPosition(x,y);
    }

    public void SwitchTile(Vector2 mousepos, TileType tp, TextureType tt, TileObject to, string asset)
    {
        //check selected tile
        Vector2 vpos = GridPosition(mousepos + new Vector2(0, cellHeight / 2));
        Point pos = new Point((int)vpos.X, (int)vpos.Y);
        Tile tile = Get(pos.X, pos.Y) as Tile;
        if (tile != null)
        {
            //change tile
            if (tile.TileObject == to)
            {
                tile.ChangeTile(tp, tt, asset);
            }
            else
            {
                //replace tile
                Remove(tile.Id, pos.X, pos.Y);
                Tile newtile;
                switch (to)
                {
                    case TileObject.Tile:
                        newtile = new Tile(pos, asset, tp, tt);
                        break;
                    case TileObject.WallTile:
                        newtile = new WallTile(pos, asset, tp, tt);
                        break;
                    case TileObject.TreeTile:
                        newtile = new TreeTile(pos, asset, tp, tt);
                        break;
                    case TileObject.GrassTile:
                        newtile = new GrassTile(pos, asset, tp, tt);
                        break;
                    default:
                        newtile = new Tile(pos);
                        break;
                }
                Add(newtile, pos.X, pos.Y);
                newtile.ChangeTile(tp, tt, asset);
            }
        }
    }
    
    public bool HasWalls(int x, int y)
    {
        if (x < 0 || x >= Columns)
        {
            return true;
        }
        if (y < 0 || y >= Rows)
        {
            return true;
        }
        Tile current = GameWorld.GetObject(Objects[x, y]) as Tile;
        return current.HasWalls;
    }

    public bool HasItem(int x, int y)
    {
        if (x < 0 || x >= Columns)
        {
            return true;
        }
        if (y < 0 || y >= Rows)
        {
            return true;
        }
        Tile current = GameWorld.GetObject(Objects[x, y]) as Tile;
        return current.HasItem;
    }

    public TileType GetTileType(int x, int y)
    {
        if (x < 0 || x >= Columns)
        {
            return TileType.Wall;
        }
        if (y < 0 || y >= Rows)
        {
            return TileType.Wall;
        }
        Tile current = GameWorld.GetObject(Objects[x, y]) as Tile;
        return current.TileType;
    }

    public TextureType GetTextureType(int x, int y)
    {
        if (x < 0 || x >= Columns)
        {
            return TextureType.None;
        }
        if (y < 0 || y >= Rows)
        {
            return TextureType.None;
        }
        Tile current = GameWorld.GetObject(Objects[x, y]) as Tile;
        if (current != null)
        {
            return current.TextureType;
        }
        else
        {
            return TextureType.None;
        }
    }

    public TileObject GetTileObject(int x, int y)
    {
        if (x < 0 || x >= Columns)
        {
            return TileObject.Tile;
        }
        if (y < 0 || y >= Rows)
        {
            return TileObject.Tile;
        }
        Tile current = GameWorld.GetObject(Objects[x, y]) as Tile;
        return current.TileObject;
    }

    public string NewPassenger(Vector2 pos, Vector2 prevPos, GameObject obj, string host)
    {
        Tile drawTile;
        (Get((int)prevPos.X, (int)prevPos.Y) as Tile).RemovePassenger(obj.Id);
        Vector2 gridPos = GridPosition(pos);
        (Get((int)gridPos.X, (int)gridPos.Y) as Tile).AddPassenger(obj.Id);
        Vector2 drawPos = DrawGridPosition(pos);
        try
        {
            drawTile = GameWorld.GetObject(grid[(int)drawPos.X, (int)drawPos.Y]) as Tile;
        }
        catch
        {
            drawTile = GameWorld.GetObject(grid[(int)drawPos.X - 1, (int)drawPos.Y]) as Tile;
        }

        drawTile.AddDrawPassenger(obj);

        if (host != "")
        {
            (GameWorld.GetObject(host) as Tile).RemoveDrawPassenger(obj.Id);
        }

        return drawTile.Id;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
    }

    public override void Update(GameTime gameTime)
    {
        List<string> entities = ActiveEnities();
        for (int i = 0; i < entities.Count; i++)
        {
            (GameWorld.GetObject(entities[i]) as Entity).Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        //draw tile with passengers
        List<string> tiles = ActiveTiles();
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile tile = GameWorld.GetObject(tiles[i]) as Tile;
            tile.Draw(gameTime, spriteBatch);

            if (GameEnvironment.GameSettingsManager.GetValue("editor") != "true")
            {
                for (int j = 0; j < tile.DrawPassengers.Count; j++)
                {
                    GameWorld.GetObject(tile.DrawPassengers[j]).Draw(gameTime, spriteBatch);
                }
            }
        }
    }

    private List<string> ActiveTiles()
    {
        List<string> tiles = new List<string>();
        Camera camera = GameWorld.GetObject("camera") as Camera;
        Vector2 cameraposbegin = GridPosition(new Vector2(camera.Screen.X, camera.Screen.Y));
        Vector2 cameraposend = GridPosition(new Vector2(camera.Screen.X + camera.Screen.Width, camera.Screen.Y + camera.Screen.Height));
        int beginz = (int)cameraposbegin.X + (int)cameraposbegin.Y;
        int endz = (int)cameraposend.X + (int)cameraposend.Y;

        for (int z = beginz; z < endz; z++)
        {
            for (int x = (int)cameraposbegin.X; x <= (int)cameraposend.X; x++)
            {
                int y = z - x;
                if (x >= grid.GetLength(0) || y >= grid.GetLength(1) || x < 0 || y < 0 || !camera.OnScreen(AnchorPosition(x, y)))
                {
                    continue;
                }
                tiles.Add(grid[x, y]);
            }
        }
        return tiles;
    }

    private List<string> ActiveEnities()
    {
        List<string> tiles = ActiveTiles();
        List<string> entities = new List<string>();
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile tile = GameWorld.GetObject(tiles[i]) as Tile;

            entities.AddRange(tile.DrawPassengers);
        }
        return entities;
    }

    public Vector2 AnchorPosition(int x, int y) //translate grid position to screen position
    {
        return new Vector2(x * cellWidth / 2 - cellWidth / 2 * y, y * cellHeight / 2 + cellHeight / 2 * x);
    }

    public Vector2 GridPosition(Vector2 pos) // translate screen position to grid position
    {
        return new Vector2((int)(pos.X / cellWidth + pos.Y / cellHeight), (int)(-pos.X / CellWidth + pos.Y / cellHeight));
    }

    public Vector2 DrawGridPosition(Vector2 pos)
    {
        if (GetTileType((int)GridPosition(pos).X + 1, (int)GridPosition(pos).Y + 1) == TileType.Wall)
        {
            return GridPosition(pos) + new Vector2(1, 0);
        }
        return GridPosition(pos) + Vector2.One;
    }
}

