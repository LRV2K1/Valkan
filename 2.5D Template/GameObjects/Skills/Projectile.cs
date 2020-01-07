using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Projectile : Item
{
    float lifetime;
    int damage;
    bool damaged;
    Rectangle hitbox;

    public Projectile(string assetname, bool animated, int boundingy, float lifetime, int damage)
        : base(assetname, animated, ItemType.InMovible, boundingy)
    {
        this.damage = damage;
        this.lifetime = lifetime;

        hitbox = BoundingBox;
        damaged = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        CheckHit();
        if (lifetime <= 0)
        {
            RemoveSelf();
            return;
        }
    }

    private void CheckHit()
    {
        List<string> surroundingentities = GetSurroundingEntities();

        hitbox = BoundingBox;
        foreach(string id in surroundingentities)
        {            
            Enemy enemy = GameWorld.GetObject(id) as Enemy;
            if (enemy != null)
            {
                if (!enemy.Dead && hitbox.Intersects(enemy.BoundingBox))
                {
                    enemy.Health -= damage;
                    damaged = true;                    
                }
            }            
        }

        if (damaged)
        {
            RemoveSelf();
        }
    }

    protected override void HandleCollisions()
    {
    }

    public Rectangle HitBox
    {
        get { return hitbox; }
        set { hitbox = value; }
    }
}

