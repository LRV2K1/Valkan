using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ItemGrid : GameObjectGrid
{
    public  ItemGrid(int collumns, int rows, int layer = 0, string id = "")
        : base(collumns, rows, layer, id)
    {

    }

    public void SetupGrid()
    {
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                EditorEntity entity = new EditorEntity(new Point(x, y), "");
                Add(entity, x, y);
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

    public void SwitchItem(Vector2 mousepos, EntityType et, string asset, int boundingy = 0)
    {
        //check selected entity
        Vector2 vpos = GridPosition(mousepos + new Vector2(0, cellHeight / 2));
        Point pos = new Point((int)vpos.X, (int)vpos.Y);
        EditorEntity entity = Get(pos.X, pos.Y) as EditorEntity;
        //replace entity
        if (entity != null)
        {
            Remove(entity.Id, pos.X, pos.Y);
            EditorEntity newentity = new EditorEntity(pos, asset, boundingy, et);
            if (et == EntityType.Enemy)
            {
                //get additional information from the mouse
                EditorMouse mouse = GameWorld.GetObject("mouse") as EditorMouse;
                newentity.EnemyType = mouse.EnemyType;
            }
            else if (et == EntityType.AnimatedItem || et == EntityType.SpriteItem)
            {
                //get additional information from the mouse
                EditorMouse mouse = GameWorld.GetObject("mouse") as EditorMouse;
                newentity.ItemType = mouse.ItemType;
            }
            Add(newentity, pos.X, pos.Y);
            newentity.InitializeTile();
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

    public override void HandleInput(InputHelper inputHelper)
    {
        List<string> entities = ActiveEntites();
        for (int i = 0; i < entities.Count; i++)
        {
            EditorEntity entity = GameWorld.GetObject(entities[i]) as EditorEntity;
            entity.HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        List<string> entities = ActiveEntites();
        for (int i = 0; i < entities.Count; i++)
        {
            EditorEntity entity = GameWorld.GetObject(entities[i]) as EditorEntity;
            entity.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        List<string> entities = ActiveEntites();
        for (int i = 0; i < entities.Count; i++)
        {
            EditorEntity entity = GameWorld.GetObject(entities[i]) as EditorEntity;
            entity.Draw(gameTime, spriteBatch);
        }
    }

    private List<string> ActiveEntites()
    {
        List<string> entities = new List<string>();
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
                entities.Add(grid[x, y]);
            }
        }
        return entities;
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

