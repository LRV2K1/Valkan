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

    public Dodge(string assetname, int skill, float timer = 1f, float speed = 2f, float runtime = 0.2f)
        : base(assetname, skill)
    {
        this.speed = speed;
        this.runtime = runtime;
        resettimer = timer;
    }

    public void Button(bool button)
    {
        Player player = parent as Player;
        if (button && timer.Ready && player.Stamina >= 20)
        {
            GameEnvironment.AssetManager.PlaySound("SFX/Player/Swoosh");
            Use(resettimer);
        }
    }

    public override void Use(float timer = 2)
    {
        Player player = parent as Player;
        base.Use(timer);
        player.Stamina -= 20;
        player.AddSpeedMultiplier(runtime, speed);
    }

    public override bool Ready
    {
        get
        {
            Player player = parent as Player;
            return timer.Ready && player.Stamina >= 20;
        }
    }
}