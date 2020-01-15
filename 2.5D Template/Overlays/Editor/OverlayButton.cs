using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class OverlayButton : TextButton
{
    public OverlayButton(string text, int layer, string id = "")
        : base("Fonts/Hud", text, layer, id)
    {

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (pressed)
        {
            OverlayStatus overlay = GameWorld.GetObject("overlay") as OverlayStatus;
            if (overlay != null)
            {
                overlay.ActiveStatus(text);
            }
        }
    }
}

