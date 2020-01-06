using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class ProjectileAttack : PrimairySkill
{
    float speed = 500;

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

            Projectile projectile = new Projectile("Sprites/Items/Projectiles/spr_ice_" + GetDirection() + "@8", true, 10, 3, normaldamage);
            projectile.Position = player.GlobalPosition;
            projectile.Sprite.Size = new Vector2(0.75f, 0.75f);
            SetProjectileDirection(projectile);
            GameWorld.RootList.Add(projectile);
        }
        else if (heavy && player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();
            Projectile projectile = new Projectile("Sprites/Items/Projectiles/spr_ice_0@8", true, 10, 3, longdamage);
            projectile.Position = player.GlobalPosition;
            SetProjectileDirection(projectile);
            GameWorld.RootList.Add(projectile);
        }
    }

    private int GetDirection()
    {
        int dir;
        Player player = parent as Player;
        double direction = player.Direction;
        if (player.Selected)
        {
            Selected icon = GameWorld.GetObject("selected") as Selected;
            float dx = icon.Position.X - player.Position.X;
            float dy = icon.Position.Y - player.Position.Y;
            direction = Math.Atan2(dy, dx);
        }

        dir = (int)((direction + (Math.PI / 8) + (3 * Math.PI / 2)) / (Math.PI / 4));
        if (dir > 7)
        {
            dir -= 8;
        }

        return dir;
    }

    private void SetProjectileDirection(Projectile projectile)
    {
        Player player = parent as Player;
        if (player.Selected)
        {
            Selected icon = GameWorld.GetObject("selected") as Selected;
            float dx = icon.Position.X - player.Position.X;
            float dy = icon.Position.Y - player.Position.Y;
            float direction = (float)Math.Sqrt(dx * dx + dy * dy);

            projectile.Velocity = new Vector2((dx / direction) * speed, (dy / direction) * speed);
        }
        else
        {
            projectile.Velocity = new Vector2(speed * (float)Math.Cos(player.Direction), speed * (float)Math.Sin(player.Direction));
        }

    }
}

