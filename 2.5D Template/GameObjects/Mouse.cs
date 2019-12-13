﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class GameMouse : SpriteGameObject
{
    Vector2 mousePos;
    public GameMouse()
        : base("Sprites/Menu/spr_mouse",200, "mouse")
    {  
    }
    
    //update mouseposition
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        Camera camera = GameWorld.GetObject("camera") as Camera;
        mousePos = inputHelper.MousePosition + camera.CameraPosition;
        position = inputHelper.MousePosition;
    }

    //select entities
    public bool SelectEntity() 
    {
        LevelGrid levelGrid = GameWorld.GetObject("tiles") as LevelGrid;
        Level level = GameWorld as Level;
        Vector2 vpos = levelGrid.GridPosition(mousePos + new Vector2(0, levelGrid.CellHeight / 2));
        Point gridpos = new Point((int)vpos.X + 1, (int)vpos.Y + 1);
        Selected icon = GameWorld.GetObject("selected") as Selected;

        string entity = "";
        float closedistance = 100f;

        //check clicked area
        for (int x = gridpos.X - 1; x <= gridpos.X + 1; x++)
        {
            for (int y = gridpos.Y - 1; y <= gridpos.Y + 1; y++)
            {
                Tile tile = levelGrid.Get(x, y) as Tile;
                if (tile == null)
                {
                    continue;
                }

                for (int i = 0; i < tile.Passengers.Count; i++)
                {
                    //check if enemy
                    Enemy enemy = GameWorld.GetObject(tile.Passengers[i]) as Enemy;
                    if (enemy == null)
                    {
                        continue;
                    }

                    //check if closest
                    if (enemy.OnSprite(mousePos) && !enemy.Dead)
                    {
                        float xd = mousePos.X - enemy.GlobalPosition.X;
                        float yd = mousePos.Y - enemy.GlobalPosition.Y;
                        float distance = (float)Math.Sqrt(xd * xd + yd * yd);
                        if (distance < closedistance)
                        {
                            entity = enemy.Id;
                            closedistance = distance;
                        }
                    }
                }
            }
        }
        //update selected icon
        if (icon == null)
        {
            if (entity != "")
            {
                icon = new Selected(1, "selected");
                level.RootList.Add(icon);
                icon.SelectedEntity = entity;

                Enemy enemy = GameWorld.GetObject(entity) as Enemy;
                enemy.Selected = true;
            }
        }
        else
        {
            if (entity == "")
            {
                Enemy previous = GameWorld.GetObject(icon.SelectedEntity) as Enemy;
                previous.Selected = false;
                level.RootList.Remove(icon.Id);
            }
            else
            {
                Enemy previous = GameWorld.GetObject(icon.SelectedEntity) as Enemy;
                previous.Selected = false;
                icon.SelectedEntity = entity;
                Enemy enemy = GameWorld.GetObject(entity) as Enemy;
                enemy.Selected = true;
            }
        }
        return entity != "";
    }

    //removeselected entity
    public void RemoveSelectedEntity()
    {
        Selected icon = GameWorld.GetObject("selected") as Selected;
        if (icon == null)
        {
            return;
        }

        Level level = GameWorld as Level;
        Enemy enemy = GameWorld.GetObject(icon.SelectedEntity) as Enemy;
        enemy.Selected = false;
        level.RootList.Remove(icon.Id);

        Player player = GameWorld.GetObject("player") as Player;
        player.Selected = false;

    }

    public Vector2 MousePos
    {
        get { return mousePos; }
    }
}