﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class CloseAttack : Skill
{
    protected float range;
    protected int damage;
    protected float resettimer;

    public CloseAttack(string assetname, int skill, float timer = 1f, int damage = 10, float range = 50)
        : base(assetname, skill)
    {
        this.range = range;
        this.damage = damage;
        resettimer = timer;
        
    }

    public override void Button(bool button)
    {
        Player player = parent as Player;
        if (button && timer.Ready && player.Stamina >= 20)
        {
            Use(resettimer);
        }
    }

    public override void Use(float timer = 2)
    {
        Player player = parent as Player;
        base.Use(timer);
        player.Stamina -= 20;
        player.SwitchAnimation("attack", "B");
        GameEnvironment.AssetManager.PlayPartySound("SFX/Player/Swoosh");
        MakeProjectile(player.GlobalPosition);
    }

    private void MakeProjectile(Vector2 position)
    {
        Projectile projectile = new Projectile("", false, damage, Vector2.Zero, "", 0.1f, "", 40, 40);
        projectile.Position = position;
        SetAttackBox(projectile);
        (GameWorld.GetObject("items") as GameObjectList).Add(projectile);
        projectile.Reset();
    }

    private void SetAttackBox(Projectile projectile)
    {
        Player player = parent as Player;
        projectile.Position += new Vector2(range * (float)Math.Cos(player.Direction), range * (float)Math.Sin(player.Direction));
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