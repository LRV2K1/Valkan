using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


class Skill : GameObject
{

    protected SkillTimer timer;
    protected Keys key;
    protected MouseButton button;

    //generic skill class
    public Skill(string assetname, MouseButton button = MouseButton.None, Keys key = Keys.None)
        : base()
    {
        timer = new SkillTimer(assetname);
        this.key = key;
        this.button = button;
    }

    //setup skill
    public void Setup()
    {
        GameWorld.Add(this);
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        Overlay hud = overlay.GetOverlay("hud") as Overlay;

        //add timer to the hud overlay
        hud.Add(timer);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (button != MouseButton.None && key != Keys.None)
        {
            if (inputHelper.MouseButtonPressed(button) && inputHelper.IsKeyDown(key) && timer.Ready)
            {
                Use();
            }
        }
        else if (button != MouseButton.None)
        {
            if (inputHelper.MouseButtonPressed(button) && timer.Ready)
            {
                Use();
            }
        }
        else if (key != Keys.None)
        {
            if (inputHelper.IsKeyDown(key) && timer.Ready)
            {
                Use();
            }
        }
    }

    public virtual void Use(float timer = 2f)
    {
        this.timer.Use(timer);
    }

    public SkillTimer Timer
    {
        get { return timer; }
    }
}

