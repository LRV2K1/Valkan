using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class PrimairySkill : Skill
{
    protected float normaltimer, longtimer;
    protected int normaldamage, longdamage;
    protected bool heavy;

    public PrimairySkill(string assetname, float normaltimer = 1f, float longtimer = 3f, int normaldamage = 10, int longdamage = 30)
        : base(assetname, MouseButton.Left, Keys.LeftShift)
    {
        this.normaltimer = normaltimer;
        this.longtimer = longtimer;
        this.normaldamage = normaldamage;
        this.longdamage = longdamage;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.MouseButtonDown(button) && inputHelper.IsKeyDown(key) && timer.Ready)
        {
            heavy = true;
            Use(longtimer);
        }
        else if (inputHelper.MouseButtonDown(button) && timer.Ready)
        {
            heavy = false;
            Use(normaltimer);
        }
    }
}