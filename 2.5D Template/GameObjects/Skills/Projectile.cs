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

    public Projectile(string assetname, bool animated, int boundingy, float lifetime, int damage)
        : base(assetname, animated, ItemType.InMovible, boundingy)
    {
        this.damage = damage;
        this.lifetime = lifetime;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (lifetime <= 0)
        {
            RemoveSelf();
        }
    }

    protected override void HandleCollisions()
    {

    }
}

