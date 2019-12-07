using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Overlay : AnimatedGameObject
{
    public Overlay() 
        : base(5, "overlay")
    {
        LoadAnimation("Sprites/Player/spr_boundingbox", "boundingbox", false, false);
        LoadAnimation("Sprites/Overlay/spr_idle", "idle_overlay", true, true);
    }

    public void PlayAnimation(string id, SpriteSheet sprite)
    {

        base.PlayAnimation(id);
        origin = new Vector2(sprite.Width / 2, sprite.Height - GetAnimation("boundingbox").Height / 2);
    }
}
