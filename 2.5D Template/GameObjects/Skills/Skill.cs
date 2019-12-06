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
    protected Keys button;

    public Skill(string assetname, Keys button)
        : base()
    {
        timer = new SkillTimer(assetname);
        this.button = button;
    }

    public void Setup()
    {
        RootList.Add(this);
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        Overlay hud = overlay.GetOverlay("hud") as Overlay;

        hud.Add(timer);

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(button) && timer.Ready)
        {
            Use();
        }
    }

    public void Use()
    {
        timer.Use(2f);
    }

    public SkillTimer Timer
    {
        get { return timer; }
    }
}

