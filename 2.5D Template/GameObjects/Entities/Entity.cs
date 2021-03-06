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
    protected string drawHost;
    protected bool remove;

    public Entity(int boundingy, int weight = 10, int layer = 0, string id = "")
        : base(layer, id)
    {
        remove = false;
        drawHost = "";
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

    public virtual void SendData(string data = "")
    {
        if (Current != null && MultiplayerManager.Online)
        {
            if (data == "")
            {
                MultiplayerManager.Party.Send("Entity: " + Id + " " + position.X + " " + position.Y + " " + origin.X + " " + origin.Y + " " + Current.AssetName + " " + Current.IsLooping + " " + Current.IsBackAndForth, MultiplayerManager.PartyPort, false);
            }
            else
            {
                MultiplayerManager.Party.Send("Entity: " + id + " " + data, MultiplayerManager.PartyPort, false);
            }
        }

    }

    public virtual void NewHost()
    {
        //become a passenger of a tile
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        //check if on new tile
        if (levelGrid.GridPosition(position) != gridpos)
        {
            drawHost = levelGrid.NewPassenger(position, gridpos, this, drawHost);
            gridpos = levelGrid.GridPosition(position);
            drawgridpos = levelGrid.DrawGridPosition(position);
        }
        else if (drawHost != "")
        {
            (GameWorld.GetObject(drawHost) as Tile).CheckDrawPassengerPosition(this);
        }
    }

    public virtual void MovePositionOnGrid(int x, int y)
    {
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        position = levelGrid.AnchorPosition(x,y);
    }

    public override void RemoveSelf()
    {
        Tile host = GameWorld.GetObject(this.drawHost) as Tile;
        if (host != null)
        {
            host.RemoveDrawPassenger(id);
        }
        if (gridpos != null)
        {
            Tile gridTile = (GameWorld.GetObject("levelgrid") as LevelGrid).Get((int)gridpos.X, (int)gridpos.Y) as Tile;
            if (gridTile != null)
            {
                gridTile.RemovePassenger(id);
            }
        }
        (parent as GameObjectList).Remove(id);
        remove = true;
        if (MultiplayerManager.Online && Current != null)
        {
            if (Current != null)
            {
                SendData("remove");
            }
        }
    }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id, isBackWards);
        SetAnimationData();
        if (MultiplayerManager.Online) //send data if online
        {
            SendData();
        }
    }

    protected virtual void SetAnimationData()
    {
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

