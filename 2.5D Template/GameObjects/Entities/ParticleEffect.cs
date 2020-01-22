using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class ParticleEffect : Item
{
    bool empty;

    public ParticleEffect(string asset, bool looping = false)
        : base(asset, looping)
    {

        if (asset == "")
        {
            empty = true;
            return;
        }

        LoadAnimation(asset, "sprite", looping, false, 0.07f);
        PlayAnimation("sprite");
    }

    public override void Update(GameTime gameTime)
    {
        if (empty)
        {
            RemoveSelf();
            return;
        }
        base.Update(gameTime);
        if (Current.AnimationEnded)
        {
            RemoveSelf();
        }
    }

    protected override void HandleCollisions() { }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id, isBackWards);
        origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
    }
}

