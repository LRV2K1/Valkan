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

            Projectile projectile = new Projectile("Sprites/Items/Projectiles/spr_ice_" + GetSpriteDirection() + "@8", true, 3, normaldamage);
            projectile.Position = player.GlobalPosition;
            if (projectile.Sprite != null)
            {
                projectile.Sprite.Size = new Vector2(0.75f, 0.75f);
            }
            SetProjectileSpeed(projectile);
            GameWorld.RootList.Add(projectile);
        }
        else if (heavy && player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();
            Projectile projectile = new Projectile("Sprites/Items/Projectiles/spr_ice_" + GetSpriteDirection() + "@8", true, 3, longdamage);
            projectile.Position = player.GlobalPosition;
            SetProjectileSpeed(projectile);
            GameWorld.RootList.Add(projectile);
        }
    }

    private void SetProjectileSpeed(Projectile projectile)
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