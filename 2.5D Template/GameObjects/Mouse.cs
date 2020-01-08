using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class GameMouse : SpriteGameObject
{
    Vector2 mousePos;
    public GameMouse()
        : base("Sprites/Menu/spr_mouse",200, "mouse")
    {

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        Camera camera = GameWorld.GetObject("camera") as Camera;
        mousePos = inputHelper.MousePosition + camera.CameraPosition;
        position = inputHelper.MousePosition;
    }

    public Vector2 MousePos
    {
        get { return mousePos; }
    }
}

