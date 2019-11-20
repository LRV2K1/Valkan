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

    public override void Add(GameObject obj, int x, int y)
    {
        GameWorld.Add(obj);
        grid[x, y] = obj.Id;
        obj.Parent = this;
        obj.Position = AnchorPosition(x,y);
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
        return current.TextureType;
    }

    public string NewPassenger(Vector2 newPos, Vector2 prevPos, GameObject obj, string host)
    {
        Tile tile;

        try
        {
            tile = GameWorld.GetObject(grid[(int)newPos.X, (int)newPos.Y]) as Tile;
        }
        catch
        {
            tile = GameWorld.GetObject(grid[(int)newPos.X - 1, (int)newPos.Y]) as Tile;
        }

        tile.AddPassenger(obj);

        if (host != "")
        {
            Tile prevtile = GameWorld.GetObject(host) as Tile;
            prevtile.RemovePassenger(obj.Id);
        }

        return tile.Id;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Camera camera = GameWorld.GetObject("camera") as Camera;
        int minLength = Math.Min(grid.GetLength(0), grid.GetLength(1));
        int maxLengt = Math.Max(grid.GetLength(0), grid.GetLength(1));
        for (int z = 0; z < grid.GetLength(0) + (grid.GetLength(1) - 1); z++)
        {
            for (int x = 0; x <= z; x++)
            {
                int y = z - x;
                if (x >= grid.GetLength(0) || y >= grid.GetLength(1) || !camera.OnScreen(AnchorPosition(x,y)))
                {
                    continue;
                }
                Tile tile = GameWorld.GetObject(grid[x, y]) as Tile;
                tile.Draw(gameTime, spriteBatch);

                for (int i = 0; i < tile.Passengers.Count; i++)
                {
                    GameWorld.GetObject(tile.Passengers[i]).Draw(gameTime, spriteBatch);
                }
            }
        }
    }

    public Vector2 AnchorPosition(int x, int y)
    {
        return new Vector2(x * cellWidth / 2 - cellWidth / 2 * y, y * cellHeight / 2 + cellHeight / 2 * x);
    }

    public Vector2 GridPosition(Vector2 pos)
    {
        return new Vector2(pos.X/cellWidth + pos.Y/cellHeight, -pos.X/CellWidth + pos.Y/cellHeight);
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

