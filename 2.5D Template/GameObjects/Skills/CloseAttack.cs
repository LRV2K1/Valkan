using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class CloseAttack : PrimairySkill
{
    protected float range;

    public CloseAttack(string assetname, float normaltimer = 1f, float longtimer = 3f, int normaldamage = 10, int longdamage = 30)
        : base(assetname, normaltimer, longtimer, normaldamage, longdamage)
    {
        range = 100;        
    }

    public override void Use(float timer = 2)
    {
        Attack(timer);
    }

    public void Attack(float timer)
    {
        Player player = parent as Player;
        if (!heavy && player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();
            Projectile projectile = new Projectile("", false, 0.1f, normaldamage, 25, 25);
            projectile.Position = player.GlobalPosition;
            SetAttackBox(projectile);
            GameWorld.RootList.Add(projectile);
        }
        else if (heavy && player.Stamina >= 20)
        {
            base.Use(timer);
            player.Stamina -= 20;
            player.AttackAnimation();
            
        }
    }

    private void SetAttackBox(Projectile projectile)
    {
        Player player = parent as Player;
        projectile.Position += new Vector2(range * (float)Math.Cos(player.Direction), range * (float)Math.Sin(player.Direction));
    }
}