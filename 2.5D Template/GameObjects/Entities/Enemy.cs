using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class Enemy : Entity
{
    protected int health;
    protected bool die, dead;
    protected bool selected;

    //generic enemy
    //no function yet
    public Enemy(string assetname, int boundingy, int weight = 200, int layer = 0, string id = "")
        : base(boundingy, weight, layer, id)
    {
        selected = false;
        dead = false;
        health = 20;
        LoadAnimation(assetname, "sprite", true, false);
        LoadAnimation(assetname, "die", false, false);
        PlayAnimation("sprite");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (die || dead)
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
            if (selected)
            {
                GameMouse mouse = GameWorld.GetObject("mouse") as GameMouse;
                mouse.RemoveSelectedEntity();
            }
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

    public bool Dead
    {
        get { return die; }
    }

    public bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }
}

