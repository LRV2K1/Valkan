using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class Dodge : TertairySkill
{
    protected float speed;
    protected float runtime;

    public Dodge(string assetname, float time = 1f, float speed = 2f, float runtime = 0.2f)
        : base(assetname, time)
    {
        this.speed = speed;
        this.runtime = runtime;
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