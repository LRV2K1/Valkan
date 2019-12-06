using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class SecondairySkill : Skill
{

    protected float time;
    protected int damage;

    public SecondairySkill(string assetname, float timer = 1f, int damage = 10)
        : base(assetname, MouseButton.Right)
    {
        this.time = timer;
        this.damage = damage;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.MouseButtonDown(button) && timer.Ready)
        {
            Use(time);
        }
    }

    public override void Use(float timer = 2)
    {
        base.Use(timer);
    }
}