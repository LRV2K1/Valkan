using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class PopUp : SpriteGameObject
{
    public bool active;

    public PopUp(string assetname, Vector2 boxSize, int layer = 108, string id = "popup") :
        base(assetname, layer, id)
    {
        active = false;
    }


}
