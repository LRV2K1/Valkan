using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

class EditorLevelGrid : GameObjectGrid
{
    public EditorLevelGrid(int collumns, int rows, int layer = 0, string id = "")
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
                Tile tile = new EditorGrassTile(new Point(x, y), "Sprites/Tiles/spr_grass_sheet_0@4x8", TileType.Floor, TextureType.Grass);
                Add(tile, x, y);
            }
        }
    }

    public override void Add(GameObject obj, int x, int y)
    {
        GameWorld.Add(obj);
        grid[x, y] = obj.Id;
        obj.Parent = this;
        obj.Position = AnchorPosition(x, y);
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
                        newtile = new EditorWallTile(pos, asset, tp, tt);
                        break;
                    case TileObject.TreeTile:
                        newtile = new EditorTreeTile(pos, asset, tp, tt);
                        break;
                    case TileObject.GrassTile:
                        newtile = new EditorGrassTile(pos, asset, tp, tt);
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

    public override void HandleInput(InputHelper inputHelper)
    {
        List<string> tiles = ActiveTiles();
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile tile = GameWorld.GetObject(tiles[i]) as Tile;
            tile.HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        List<string> tiles = ActiveTiles();
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile tile = GameWorld.GetObject(tiles[i]) as Tile;
            tile.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        List<string> tiles = ActiveTiles();
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile tile = GameWorld.GetObject(tiles[i]) as Tile;
            tile.Draw(gameTime, spriteBatch);
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

    public Vector2 AnchorPosition(int x, int y)
    {
        return new Vector2(x * cellWidth / 2 - cellWidth / 2 * y, y * cellHeight / 2 + cellHeight / 2 * x);
    }

    public Vector2 GridPosition(Vector2 pos)
    {
        return new Vector2(pos.X / cellWidth + pos.Y / cellHeight, -pos.X / CellWidth + pos.Y / cellHeight);
    }
}