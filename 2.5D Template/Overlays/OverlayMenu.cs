using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class OverlayMenu : Overlay
{
    protected Button invetory, skills, button3, button4, intel, exit;

    public OverlayMenu(GameObjectLibrary gameworld, int layer = 101, string id = "")
        : base(gameworld, layer, id)
    {
        Add(new SpriteGameObject("Sprites/Menu/spr_background", 101));

        invetory = new Button("Sprites/Menu/spr_button_inventory", 102);
        invetory.Position = new Vector2(80, 80);
        Add(invetory);
        skills = new Button("Sprites/Menu/spr_button_skills", 102);
        skills.Position = new Vector2(80 + invetory.Width, 80);
        Add(skills);
        button3 = new Button("Sprites/Menu/spr_button", 102);
        button3.Position = new Vector2(80 + invetory.Width * 2, 80);
        Add(button3);
        button4 = new Button("Sprites/Menu/spr_button", 102);
        button4.Position = new Vector2(80 + invetory.Width * 3, 80);
        Add(button4);
        intel = new Button("Sprites/Menu/spr_button_intel", 102);
        intel.Position = new Vector2(80 + invetory.Width * 4, 80);
        Add(intel);

        exit = new Button("Sprites/Menu/spr_button_exit");
        exit.Position = new Vector2(1488, 880);
        Add(exit);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        if (inputHelper.KeyPressed(Keys.I) || exit.Pressed)
        {
            overlay.SwitchTo("hud");
        }

        if (invetory.Pressed)
        {
            overlay.SwitchTo("inventory");
        }

        if (skills.Pressed)
        {

        }

        if (button3.Pressed)
        {

        }

        if (button4.Pressed)
        {

        }

        if (intel.Pressed)
        {

        }
    }
}

