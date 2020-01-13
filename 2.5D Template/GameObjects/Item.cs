using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Item : Entity
{

    public Item(string assetname = "Sprites/Items/spr_barrel_boundingbox", int weight = 10, int layer = -1, string id = "")
        : base (assetname, weight, layer, id)
    {
        LoadAnimation("Sprites/Items/spr_barrel", "sprite", false);
        PlayAnimation("sprite");

        origin = new Vector2(Width / 2, Height - BoundingBox.Height / 2);
    }
}

