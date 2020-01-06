using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class ProjectileAttack : PrimairySkill
{
    public ProjectileAttack(string assetname, float normaltimer = 1f, float longtimer = 3f, int normaldamage = 10, int longdamage = 30)
        : base(assetname, normaltimer, longtimer, normaldamage, longdamage)
    {

    }

    public override void Use(float timer = 2)
    {
        Attack(timer);
    }

    private void Attack(float timer)
    {
        Player player = parent as Player;
        if (!heavy && player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();
            Projectile projectile = new Projectile("Sprites/Items/Projectiles/spr_ice_0@8", true, 10, 3, 10);
            projectile.Position = player.GlobalPosition;
            projectile.Velocity = new Vector2(0, 200);
            GameWorld.RootList.Add(projectile);
        }
        else if (heavy && player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();
        }
    }
}

