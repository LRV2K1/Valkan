using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

enum ItemType
{
    InMovible,
    Movible
}

class Item : Entity
{
    protected ItemType itemtype;

    public Item(string asset = "Sprites/Items/spr_test_1", bool animated = false, ItemType it = ItemType.InMovible, int boundingy = 0, int weight = 10, int layer = -1, string id = "")
        : base (boundingy, weight, layer, id)
    {
        LoadAnimation(asset, "sprite", animated);
        PlayAnimation("sprite");
        itemtype = it;
    }

    public ItemType ItemType
    {
        get { return itemtype; }
    }

}

