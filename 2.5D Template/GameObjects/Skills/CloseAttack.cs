using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CloseAttack : PrimairySkill
{
    public CloseAttack(string assetname, float normaltimer = 1f, float longtimer = 3f, int normaldamage = 10, int longdamage = 30)
        : base(assetname, normaltimer, longtimer, normaldamage, longdamage)
    {

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
            Selected selected = GameWorld.GetObject("selected") as Selected;
            if (selected != null)
            {
                Enemy enemy = GameWorld.GetObject(selected.SelectedEntity) as Enemy;
                if (Math.Abs(Math.Sqrt((player.GlobalPosition.X - enemy.GlobalPosition.X)* (player.GlobalPosition.X - enemy.GlobalPosition.X) + (player.GlobalPosition.Y - enemy.GlobalPosition.Y) * (player.GlobalPosition.Y - enemy.GlobalPosition.Y))) < 200)
                {
                    enemy.Health -= 10;
                }
            }
        }
    }
}

