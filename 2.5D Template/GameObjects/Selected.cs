using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class Selected : SpriteGameObject
{
    public Selected(int layer = 1, string id = "")
        : base("Sprites/Player/spr_selected", layer, id)
    {
        origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
    }
}

