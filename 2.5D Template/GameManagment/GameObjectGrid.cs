using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectGrid : GameObject
{
    protected string[,] grid;
    protected int cellWidth, cellHeight;

    public GameObjectGrid(int columns, int rows, int layer = 0, string id = "")
        : base(layer, id)
    {
        grid = new string[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                grid[x, y] = "";
            }
        }
    }

    public virtual void Add(GameObject obj, int x, int y)
    {
        GameWorld.Add(obj);
        grid[x, y] = obj.Id;
        obj.Parent = this;
        obj.Position = new Vector2(x * cellWidth, y * cellHeight);
    }

    public GameObject Get(int x, int y)
    {
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
        {
            return GameWorld.GetObject(grid[x, y]);
        }
        else
        {
            return null;
        }
    }

    public string[,] Objects
    {
        get
        {
            return grid;
        }
    }

    public int Rows
    {
        get { return grid.GetLength(1); }
    }

    public int Columns
    {
        get { return grid.GetLength(0); }
    }

    public int CellWidth
    {
        get { return cellWidth; }
        set { cellWidth = value; }
    }

    public int CellHeight
    {
        get { return cellHeight; }
        set { cellHeight = value; }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        foreach (string id in grid)
        {
            GameWorld.GetObject(id).HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (string id in grid)
        {
            GameWorld.GetObject(id).Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (string id in grid)
        {
            GameWorld.GetObject(id).Draw(gameTime, spriteBatch);
        }
    }

    public override void Reset()
    {
        base.Reset();
        foreach (string id in grid)
        {
            GameWorld.GetObject(id).Reset();
        }
    }
}
