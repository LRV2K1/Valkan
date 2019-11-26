using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


partial class Entity : AnimatedGameObject
{
    protected Vector2 gridPos;
    protected Texture2D boundingbox;
    protected Vector2 previousPos;
    protected int weight;
    protected string host;

    public Entity(string boundingbox, int weight = 10, int layer = 0, string id = "")
        : base(layer, id)
    {
        host = "";
        this.weight = weight;
        this.boundingbox = GameEnvironment.AssetManager.GetSprite(boundingbox);
        previousPos = position;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (previousPos != position)
        {
            NewHost();
            previousPos = position;
            DoPhysics();
        }
    }

    public override void Reset()
    {
        base.Reset();
        NewHost();
    }

    private void NewHost()
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        if (levelGrid.DrawGridPosition(position) != gridPos)
        {
            host = levelGrid.NewPassenger(levelGrid.DrawGridPosition(position), gridPos, this, host);
            gridPos = levelGrid.DrawGridPosition(position);
        }
        else if (host != "")
        {
            (GameWorld.GetObject(host) as Tile).CheckPassengerPosition(this);
        }
    }

    public void MovePositionOnGrid(int x, int y)
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        position = new Vector2(x * levelGrid.CellWidth / 2 - levelGrid.CellWidth / 2 * y, y * levelGrid.CellHeight / 2 + levelGrid.CellHeight / 2 * x);
    }

    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - boundingbox.Width / 2);
            int top = (int)(GlobalPosition.Y - boundingbox.Height / 2);
            return new Rectangle(left, top, boundingbox.Width, boundingbox.Height);
        }
    }

    public Vector2 GridPos
    {
        get { return gridPos; }
        set { gridPos = value; }
    }

    public int Weight
    {
        get { return weight; }
    }
}

