using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class ProjectileAttack : Skill
{
    float speed = 500;
    int damage;
    float resettimer;

    string prj_asset, prj_ex_asset;
    int number_frames;
    bool directional;

    public ProjectileAttack(string assetname, string prj_asset = "", int prj_frames = 1, string prj_ex_asset = "", float timer = 1f, int damage = 10, MouseButton button = MouseButton.Left)
        : base(assetname, button)
    {
        resettimer = timer;
        this.damage = damage;

        this.prj_asset = prj_asset;
        this.prj_ex_asset = prj_ex_asset;

        number_frames = prj_frames;
        if (prj_asset != "")
        {
            directional = prj_asset[prj_asset.Length - 1] == '_';
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.MouseButtonPressed(button) && timer.Ready)
        {
            Use(resettimer);
        }
    }

    public override void Use(float timer = 2f)
    {
        Player player = parent as Player;
        if (player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();

            MakeProjectile(player.GlobalPosition);
        }
    }

    private void MakeProjectile(Vector2 position)
    {
        string prj_sprite = prj_asset;
        bool animated = false;

        if (directional)
        {
            prj_sprite += GetSpriteDirection();
        }
        if (number_frames > 1)
        {
            prj_sprite += "@" + number_frames;
            animated = true;
        }

        Projectile projectile = new Projectile(prj_sprite, animated, damage, new Vector2(0, 50) ,3, prj_ex_asset);
        projectile.Position = position;
        SetProjectileSpeed(projectile);
        GameWorld.RootList.Add(projectile);
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