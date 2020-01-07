using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class ParticleEffect : Item
{
    public ParticleEffect(string asset)
        : base(asset, false)
    {

        if (asset != "")
        {
            LoadAnimation(asset, "sprite", false, false, 0.07f);
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

