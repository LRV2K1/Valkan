using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class ParticleEffect : Item
{
    public ParticleEffect(string asset, bool looping = false)
        : base(asset, looping)
    {

        if (asset != "")
        {
            LoadAnimation(asset, "sprite", looping, false, 0.07f);
            PlayAnimation("sprite");
        }

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (Current.AnimationEnded)
        {
            RemoveSelf();
        }
    }
}

