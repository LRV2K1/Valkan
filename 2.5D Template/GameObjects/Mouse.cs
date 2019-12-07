using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class GameMouse : GameObject
{
    public GameMouse()
        : base(0, "mouse")
    {

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        Camera camera = GameWorld.Find("camera") as Camera;
        position = inputHelper.MousePosition + camera.CameraPosition;
    }
}

