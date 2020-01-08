using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class Dodge : Skill
{
    protected float speed;
    protected float runtime;
    protected float resettimer;

    public Dodge(string assetname, float timer = 1f, float speed = 2f, float runtime = 0.2f, Keys keys = Keys.Space)
        : base(assetname, MouseButton.None, keys)
    {
        this.speed = speed;
        this.runtime = runtime;
        resettimer = timer;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(key) && timer.Ready)
        {
            Use(resettimer);
        }
    }

    public override void Use(float timer = 2)
    {
        Player player = parent as Player;
        if (player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AddSpeedMultiplier(runtime, speed);
        }
    }
}