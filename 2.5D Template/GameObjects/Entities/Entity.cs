using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


abstract partial class Entity : AnimatedGameObject
{
    int count;
    protected Vector2 gridPos;
    protected int boundingy;
    protected Vector2 previousPos;
    protected int weight;
    protected string host;
    bool remove;
    string previousdata;

    public Entity(int boundingy, int weight = 10, int layer = 0, string id = "")
        : base(layer, id)
    {
        remove = false;
        host = "";
        previousdata = "";
        this.weight = weight;
        this.boundingy = boundingy;
        previousPos = position;
        previousdata = MultiplayerManager.party.GetReceivedData();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //check if moved
        if (previousPos != position)
        {
            if (MultiplayerManager.online) //send data if online
            {
                SendData();
            }
            DoPhysics();
            if (remove)
            {
                return;
            }
            NewHost();
            previousPos = position;
        }
        if (MultiplayerManager.online)
        {
            ReceiveData();
        }
        ReceiveData();
    }

    public override void Reset()
    {
        base.Reset();
        OutsideLevel();
        if (remove)
        {
            return;
        }
        NewHost();
    }

    private void SendData()
    {
        MultiplayerManager.party.Send("Entity: " + id + " " + position.X + " " + position.Y, 9999);
    }

    private void ReceiveData()
    {
        try
        {
            if (previousdata != MultiplayerManager.party.GetReceivedData())
            {
                previousdata = MultiplayerManager.party.GetReceivedData();
                string[] variables = MultiplayerManager.party.GetReceivedData().Split(' '); //split data in Type, ID, posX, posY respectively
                if (variables[0] == "Entity:" && variables[1] == id)
                {
                    count++;
                    Console.WriteLine(count);
                    position.X = float.Parse(variables[2]);
                    position.Y = float.Parse(variables[3]);
                    previousPos = position;
                }
            }

        }
        catch
        {

        }
    }
    private void NewHost()
    {
        //become a passenger of a tile
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
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
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        position = new Vector2(x * levelGrid.CellWidth / 2 - levelGrid.CellWidth / 2 * y, y * levelGrid.CellHeight / 2 + levelGrid.CellHeight / 2 * x);
    }

    public override void RemoveSelf()
    {
        Tile host = GameWorld.GetObject(this.host) as Tile;
        if (host != null)
        {
            host.RemovePassenger(id);
        }
        (parent as GameObjectList).Remove(id);
        remove = true;
    }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id, isBackWards);
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

