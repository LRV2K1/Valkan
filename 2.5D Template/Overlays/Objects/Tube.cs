using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Tube : SpriteGameObject
{

    SpriteGameObject substance;
    protected float growspeed;
    protected float targetSize;
    public Tube(string substance, int layer = 101, string id = "")
        : base("Sprites/Menu/spr_tube", layer, id)
    {
        Origin = new Vector2(Width / 2, Height);
        this.substance = new SpriteGameObject(substance, 101);
        this.substance.Parent = this;
        this.substance.Origin = new Vector2(this.substance.Width / 2, this.substance.Height);
        this.substance.Position = new Vector2(0, -(Height - this.substance.Height) / 2);
        targetSize = 1;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //update size
        growspeed = (targetSize - substance.Sprite.Size.Y) * 0.1f;
        substance.Sprite.Size += new Vector2(0, growspeed);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        if (visible)
        {
            substance.Draw(gameTime, spriteBatch);
        }
    }

    public override void Reset()
    {
        base.Reset();
        targetSize = 1;
    }

    public float TargetSize
    {
        set { targetSize = value; }
    }
}

