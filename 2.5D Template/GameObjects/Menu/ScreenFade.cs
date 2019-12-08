using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

class ScreenFade : SpriteGameObject
{
    public void ScreenFade(string assetname, int layer = 101, string id = "") :
        base(assetname, layer, id)
    {

    }
}

