using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class Enemy : Entity
{
    protected int health;
    public bool die, dead;

    public Enemy(string assetname, int boundingy, int weight = 200, int layer = 0, string id = "")
        : base(boundingy, weight, layer, id)
    {
        dead = false;
        health = 20;
        LoadAnimation(assetname, "sprite", true);
        LoadAnimation(assetname, "die", false);
        PlayAnimation("sprite");
    }

    public override void Update(GameTime gameTime)
    {
        if (dead)
        {
            return;
        }
        base.Update(gameTime);
        if (die)
        {
            if (Current.AnimationEnded)
            {
                dead = true;
            }
            return;
        }
    }

    private void CheckDie()
    {
        if (health <=0)
        {
            die = true;
            PlayAnimation("die");
            sprite.Color = Color.Pink;
        }
    }

    public int Health
    {
        get { return health; }
        set { 
            health = value;
            CheckDie();
        }
    }
}

