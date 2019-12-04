using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Item : Entity
{

    public Item(string asset = "Sprites/Items/spr_test_1", int boundingy = 0, int weight = 10, int layer = -1, string id = "")
        : base (boundingy, weight, layer, id)
    {
        LoadAnimation(asset, "sprite", true,  0.2f);
        PlayAnimation("sprite");

        origin = new Vector2(Width / 2, Height - BoundingBox.Height / 2);
    }
}

