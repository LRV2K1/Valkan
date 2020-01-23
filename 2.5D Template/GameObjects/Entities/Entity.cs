﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


abstract partial class Entity : AnimatedGameObject
{
    protected Vector2 drawgridpos, gridpos;
    protected int boundingy;
    protected Vector2 previousPos;
    protected int weight;
    protected string host;
    protected bool remove;

    public Entity(int boundingy, int weight = 10, int layer = 0, string id = "")
        : base(layer, id)
    {
        remove = false;
        host = "";
        this.weight = weight;
        this.boundingy = boundingy;
        previousPos = position;

        if (MultiplayerManager.Online) //send data if online
        {
            SendData();
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        //check if moved
        if (previousPos != position)
        {
            if (MultiplayerManager.Online) //send data if online
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

    public virtual void SendData()
    {
        if (Current != null)
        {
            MultiplayerManager.Party.Send("Entity: " + Id + " " + position.X + " " + position.Y + " " + origin.X + " " + origin.Y + " " + Current.AssetName + " " + Current.IsLooping + " " + Current.IsBackAndForth, MultiplayerManager.PartyPort, false);
        }
    }

    public virtual void NewHost()
    {
        //become a passenger of a tile
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        //check if on new tile
        if (levelGrid.GridPosition(position) != gridpos)
        {
            gridpos = levelGrid.GridPosition(position);
            host = levelGrid.NewPassenger(levelGrid.DrawGridPosition(position), drawgridpos, this, host);
            drawgridpos = levelGrid.DrawGridPosition(position);
        }
        else if (host != "")
        {
            (GameWorld.GetObject(host) as Tile).CheckPassengerPosition(this);
        }
    }

    public virtual void MovePositionOnGrid(int x, int y)
    {
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        position = levelGrid.AnchorPosition(x,y);
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
        if (MultiplayerManager.Online)
        {
            if (Current != null)
            {
                MultiplayerManager.Party.Send("Entity: " + id + " remove", MultiplayerManager.PartyPort, false);
            }
        }
    }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id, isBackWards);
        origin = new Vector2(sprite.Width / 2, sprite.Height - BoundingBox.Height / 2);
        if (MultiplayerManager.Online) //send data if online
        {
            SendData();
        }
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

    public Vector2 DrawGridPos
    {
        get { return drawgridpos; }
        set { drawgridpos = value; }
    }

    public Vector2 GridPos
    {
        get { return gridpos; }
        set { gridpos = value; }
    }

    public int Weight
    {
        get { return weight; }
    }
}

