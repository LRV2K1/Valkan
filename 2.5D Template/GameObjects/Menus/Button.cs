using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Button : SpriteGameObject
{
    protected int buttonID;
    protected int buttonText;
    protected int destinationScene;
    public Button(string assetname = "Content/Sprites/Overlay/Menu_BG_Grey.jpg") :
        base(assetname, 101, "button")
    {
        
    }
}

