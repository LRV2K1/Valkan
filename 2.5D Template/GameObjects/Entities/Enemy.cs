﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class Enemy : Entity
{
    protected int health;
    protected bool die, dead;
    protected bool selected;

    public Enemy(string assetname, int boundingy, int weight = 200, int layer = 0, string id = "")
        : base(boundingy, weight, layer, id)
    {
        selected = false;
        dead = false;
        health = 20;
        LoadAnimation(assetname, "sprite", true, false);
        LoadAnimation(assetname, "zombie_death_0", false, false);
        PlayAnimation("sprite");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CheckDie();
        if (die || dead)
        {
            if (Current.AnimationEnded)
            {
                dead = true;
            }
            return;
        }
        health--;
    }

    private void CheckDie()
    {
        if (health <= 0)
        {
            die = true;
            PlayAnimation("zombie_death_0");
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
        set
        {
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
