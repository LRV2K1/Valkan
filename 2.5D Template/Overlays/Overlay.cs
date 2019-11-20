using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Overlay : GameObjectList
{
    public Overlay (GameObjectLibrary gameworld, int layer = 101, string id = "")
        : base (layer, id)
    {
        GameWorld = gameworld;
    }
}

