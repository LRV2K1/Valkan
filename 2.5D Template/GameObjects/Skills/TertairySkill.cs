using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class TertairySkill : Skill
{
    protected float time;
    public TertairySkill(string assetname, float time = 1f)
        : base(assetname, MouseButton.None, Keys.Space)
    {
        this.time = time;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(key) && timer.Ready)
        {
            Use(time);
        }
    }
}