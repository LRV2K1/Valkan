using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class Enemy : Entity
{
    public Enemy(string assetname, int boundingy, int weight, int layer = 0, string id = "")
        : base(boundingy, weight, layer, id)
    {
        LoadAnimation(assetname, "sprite", true);
        PlayAnimation("sprite");

        origin = new Vector2(Width / 2, Height - BoundingBox.Height / 2);
    }
}

