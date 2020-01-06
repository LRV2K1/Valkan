using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


abstract partial class Entity : AnimatedGameObject
{
    protected Vector2 gridPos;
    protected int boundingy;
    protected Vector2 previousPos;
    protected int weight;
    protected string host;

    public Entity(int boundingy, int weight = 10, int layer = 0, string id = "")
        : base(layer, id)
    {
        host = "";
        this.weight = weight;
        this.boundingy = boundingy;
        previousPos = position;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        //check if moved
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
        //become a passenger of a tile
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        //check if on new tile
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

    public virtual void MovePositionOnGrid(int x, int y)
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        position = new Vector2(x * levelGrid.CellWidth / 2 - levelGrid.CellWidth / 2 * y, y * levelGrid.CellHeight / 2 + levelGrid.CellHeight / 2 * x);
    }

    public override void RemoveSelf()
    {
        Tile host = GameWorld.GetObject(this.host) as Tile;
        host.RemovePassenger(id);
        (parent as GameObjectList).Remove(id);
    }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id);
        origin = new Vector2(sprite.Width / 2, sprite.Height - BoundingBox.Height / 2);
    }

    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - boundingy);
            int top = (int)(GlobalPosition.Y - boundingy/2);
            return new Rectangle(left, top, boundingy*2, boundingy);
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

