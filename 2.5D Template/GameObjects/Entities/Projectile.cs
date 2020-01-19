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
    string particle_asset;
    Vector2 offsetposition;

    public Projectile(string assetname, bool animated, int damage, Vector2 offsetposition, float lifetime = 3f, string part_asset = "", int hitboxX = 10, int hitboxY = 10)
        : base(assetname, animated)
    {
        this.damage = damage;
        this.lifetime = lifetime;
        this.offsetposition = offsetposition;

        particle_asset = part_asset;

        hitbox = new Point(hitboxX, hitboxY);
        damaged = false;

        origin += offsetposition;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (remove)
        {
            return;
        }
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
            if (particle_asset != "")
            {
                ParticleEffect particleEffect = new ParticleEffect(particle_asset);
                particleEffect.Position = GlobalPosition;
                particleEffect.Origin += offsetposition;
                GameWorld.RootList.Add(particleEffect);
            }
            RemoveSelf();
        }
    }

    protected override void HandleCollisions() { }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id, isBackWards);
        origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        origin += offsetposition;
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