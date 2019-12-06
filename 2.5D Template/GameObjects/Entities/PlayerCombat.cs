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
        skill1.HandleInput(inputHelper);
    }

    public void RemoveSelectedEntity()
    {
        Selected icon = GameWorld.GetObject("selected") as Selected;
        GameWorld.GetObject(icon.SelectedEntity).RemoveSelf();
        Level level = GameWorld as Level;
        level.RootList.Remove(icon.Id);
        selected = false;
    }
}