using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

partial class Player : Entity
{
    private void EntitySelection(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Z))
        {
            GameMouse mouse = GameWorld.GetObject("mouse") as GameMouse;
            string entity = mouse.CeckEntitySelected();
            if (entity != "")
            {
                if (selected)
                {
                    Selected icon = GameWorld.GetObject("selected") as Selected;
                    icon.SelectedEntity = entity;
                }
                else
                {
                    Selected icon = new Selected(1, "selected");
                    Level level = GameWorld as Level;
                    level.RootList.Add(icon);
                    icon.SelectedEntity = entity;
                    selected = true;
                }
            }
            else if (selected)
            {
                Selected icon = GameWorld.GetObject("selected") as Selected;
                Level level = GameWorld as Level;
                level.RootList.Remove(icon.Id);
                selected = false;
            }
        }
    }

    private void Skills(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Q))
        {
            Health -= 3;
        }
        skill1.HandleInput(inputHelper);
        skill2.HandleInput(inputHelper);
        skill3.HandleInput(inputHelper);
    }

    public void RemoveSelectedEntity()
    {
        Selected icon = GameWorld.GetObject("selected") as Selected;
        GameWorld.GetObject(icon.SelectedEntity).RemoveSelf();
        Level level = GameWorld as Level;
        level.RootList.Remove(icon.Id);
        selected = false;
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (skill2.Blocking)
            {
                skill2.Blocking = false;
                return;
            }
            health = value;
            if (health > maxhealth)
            {
                health = maxhealth;
            }
            else if (health < 0)
            {
                health = 0;
            }
        }
    }

    public int Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            if (stamina > maxstamina)
            {
                stamina = maxstamina;
            }
            else if (stamina < 0)
            {
                stamina = 0;
            }
        }
    }

    public int MaxHealth
    {
        get { return maxhealth; }
        set
        {
            maxhealth = value;
            Health = health;
        }
    }

    public int MaxStamina
    {
        get { return maxstamina; }
        set
        {
            maxstamina = value;
            Stamina = stamina;
        }
    }

    public bool Selected
    {
        get { return selected; }
    }
}