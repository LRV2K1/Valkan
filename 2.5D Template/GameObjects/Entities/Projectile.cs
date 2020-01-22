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
    string explosionsound;

    public Projectile(string assetname, bool animated, int damage, Vector2 offsetposition, string sound = "", float lifetime = 3f, string part_asset = "", int hitboxX = 10, int hitboxY = 10)
        : base(assetname, animated)
    {
        this.damage = damage;
        this.lifetime = lifetime;
        this.offsetposition = offsetposition;
        this.explosionsound = sound;
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
            Explode();
            return;
        }
        CheckHit();
    }

    protected void CheckHit()
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
                    GameEnvironment.AssetManager.PlaySound("SFX/Player/Thud");
                    if (enemy.Health > 0)
                    {
                        GameEnvironment.AssetManager.PlaySound(enemy.Damage_Sound);
                    }
                    damaged = true;                    
                }
            }            
        }

        if (damaged)
        {
            Explode();
        }
    }

    protected override void HandleCollisions() 
    {
        List<string> surroundingTiles = GetSurroundingTiles();
        for (int i = 0; i < surroundingTiles.Count; i++)
        {
            Tile tile = GameWorld.GetObject(surroundingTiles[i]) as Tile;
            if (tile.TileType == TileType.Wall && tile.TextureType != TextureType.Water)
            {
                Rectangle tilebounds = tile.GetBoundingBox();
                if (tilebounds.Intersects(BoundingBox))
                {
                    Explode();
                    return;
                }
            }
        }
    }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id, isBackWards);
        origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        origin += offsetposition;
    }

    protected void Explode()
    {
        if (particle_asset != "")
        {
            ParticleEffect particleEffect = new ParticleEffect(particle_asset);
            particleEffect.Position = GlobalPosition;
            particleEffect.Origin += offsetposition;
            (GameWorld.GetObject("items") as GameObjectList).Add(particleEffect);
            particleEffect.Reset();
            GameEnvironment.AssetManager.PlaySound(explosionsound);
        }
        RemoveSelf();
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