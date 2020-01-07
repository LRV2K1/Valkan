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
    Point hitbox;

    public Projectile(string assetname, bool animated, float lifetime, int damage, int hitboxX = 10, int hitboxY = 10)
        : base(assetname, animated)
    {
        this.damage = damage;
        this.lifetime = lifetime;

        hitbox = new Point(hitboxX, hitboxY);
        damaged = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (lifetime <= 0)
        {
            RemoveSelf();
            return;
        }
        CheckHit();
    }

    private void CheckHit()
    {
        List<string> surroundingentities = GetSurroundingEntities();

        foreach(string id in surroundingentities)
        {            
            Enemy enemy = GameWorld.GetObject(id) as Enemy;
            if (enemy != null)
            {
                if (!enemy.Dead && HitBox.Intersects(enemy.BoundingBox))
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

    private Rectangle HitBox
    {
        get 
        {
            return new Rectangle((int)(GlobalPosition.X - hitbox.X/2), (int)(GlobalPosition.Y - hitbox.Y/2), hitbox.X, hitbox.Y); 
        }
    }

    public Point SetHitBoxSize
    {
        get { return hitbox; }
        set { hitbox = value; }
    }
}

