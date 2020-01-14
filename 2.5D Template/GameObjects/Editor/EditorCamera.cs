using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class testcamera : GameObject
{
    Vector2 cameraPosition;
    Vector2 cameraSpeed;
    int width, height;
    const int edge = 300;

    public testcamera(int layer = 0, string id = "camera")
        : base(layer, id)
    {
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.IsKeyDown(Keys.Left))
        {
            cameraSpeed.X = -800;
        }
        else if (inputHelper.IsKeyDown(Keys.Right))
        {
            cameraSpeed.X = 800;
        }
        else
        {
            cameraSpeed.X = 0;
        }

        if (inputHelper.IsKeyDown(Keys.Up))
        {
            cameraSpeed.Y = -800;
        }
        else if (inputHelper.IsKeyDown(Keys.Down))
        {
            cameraSpeed.Y = 800;
        }
        else
        {
            cameraSpeed.Y = 0;
        }
    }

    public override void Update(GameTime gameTime)
    {
        cameraPosition += cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public int Width
    {
        get { return width; }
        set { width = value; }
    }

    public int Height
    {
        get { return height; }
        set { height = value; }
    }

    public Vector2 CameraPosition
    {
        get { return cameraPosition; }
    }

    public Vector2 SetupCamera
    {
        set { cameraPosition = value; }
    }

    public bool OnScreen(Vector2 loc)
    {
        Rectangle screen = new Rectangle((int)cameraPosition.X - edge, (int)cameraPosition.Y - edge, GameEnvironment.Screen.X + edge * 2, GameEnvironment.Screen.Y + edge * 2);
        return screen.Contains(new Point((int)loc.X, (int)loc.Y));
    }

    public Rectangle Screen
    {
        get { return new Rectangle((int)cameraPosition.X - edge, (int)cameraPosition.Y - edge, GameEnvironment.Screen.X + edge * 2, GameEnvironment.Screen.Y + edge * 2); }
    }
}