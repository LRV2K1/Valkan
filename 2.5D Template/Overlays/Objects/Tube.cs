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
        Origin = new Vector2(Width, Height / 2);
        this.substance = new SpriteGameObject(substance, 101);
        this.substance.Parent = this;
        this.substance.Origin = new Vector2(this.substance.Width, this.substance.Height / 2);
        this.substance.Position = new Vector2(-14, 0);
        targetSize = 1;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //update size
        float cutSize = (1-targetSize) * substance.Sprite.Width;
        growspeed = (cutSize - substance.Sprite.Cut.X) * 0.1f;
        Rectangle new_size = substance.Sprite.Cut;
        if (growspeed < 1 && growspeed >= 0.1f)
        {
            growspeed = 1;
        }
        else if (growspeed < 0.1f)
        {
            new_size.X = (int)cutSize;
        }
        new_size.X += (int)growspeed;
        substance.Sprite.Cut = new_size;
        substance.Position = new Vector2(-14 + new_size.X, 0);
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