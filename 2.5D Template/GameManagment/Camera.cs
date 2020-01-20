using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Camera : GameObject
{
    Vector2 cameraPosition;
    Vector2 cameraSpeed;
    int width, height;
    bool first;
    string objid;
    const int edge = 300;
    bool follow;

    public Camera(string folowObjid = "", int layer = 0, string id = "camera")
        : base(layer, id)
    {
        follow = !(folowObjid == "");
        first = false;
        objid = folowObjid;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.IsKeyDown(Keys.A))
        {
            cameraSpeed.X = -800;
        }
        else if (inputHelper.IsKeyDown(Keys.D))
        {
            cameraSpeed.X = 800;
        }
        else
        {
            cameraSpeed.X = 0;
        }

        if (inputHelper.IsKeyDown(Keys.W))
        {
            cameraSpeed.Y = -800;
        }
        else if (inputHelper.IsKeyDown(Keys.S))
        {
            cameraSpeed.Y = 800;
        }
        else
        {
            cameraSpeed.Y = 0;
        }
    }

    public void GetData(string data)
    {
        string[] splitdata = data.Split(' ');
        cameraPosition = new Vector2(float.Parse(splitdata[2]), float.Parse(splitdata[3]));
    }

    public override void Update(GameTime gameTime)
    {
        if (MultiplayerManager.Online)
        {
            if (GameEnvironment.GameSettingsManager.GetValue("host") == "false")
            {
                return;
            }
            else
            {
                MultiplayerManager.Party.Send("Camera: " + id + " " + cameraPosition.X + " " + cameraPosition.Y, MultiplayerManager.PartyPort, false);
            }
        }
        if (!follow)
        {
            cameraPosition += cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        GameObject folowObj = GameWorld.GetObject(objid);
        if (folowObj == null)
        {
            return;
        }

        //smooth camera
        if (!first)
        {
            cameraPosition.X = folowObj.Position.X - GameEnvironment.Screen.X / 2;
        }
        DelayPositionX(folowObj.Position.X - GameEnvironment.Screen.X / 2);


        if (!first)
        {
            cameraPosition.Y = folowObj.Position.Y - GameEnvironment.Screen.Y / 2;
        }
        DelayPositionY(folowObj.Position.Y - GameEnvironment.Screen.Y / 2);

        first = true;
    }

    public override void Reset()
    {
        first = false;
    }

    public float DelayPositionX(float newPositionX, int amount=20)
    {
        if(Math.Abs(cameraPosition.X - newPositionX) != 0)
        {
            if(cameraPosition.X - newPositionX < 0)
            {
                return cameraPosition.X += Math.Abs(cameraPosition.X - newPositionX) / amount;
            }
            else
            {
                return cameraPosition.X -= Math.Abs(cameraPosition.X - newPositionX) / amount;
            }
        }
        return newPositionX;
    }

    public float DelayPositionY(float newPositionY, int amount = 20)
    {
        if (Math.Abs(cameraPosition.Y - newPositionY) != 0)
        {
            if (cameraPosition.Y - newPositionY < 0)
            {
                return cameraPosition.Y += Math.Abs(cameraPosition.Y - newPositionY) / amount;
            }
            else
            {
                return cameraPosition.Y -= Math.Abs(cameraPosition.Y - newPositionY) / amount;
            }
        }
        return newPositionY;
    }

    public bool OnScreen(Vector2 pos)
    {
        Rectangle screen = new Rectangle((int)cameraPosition.X - edge, (int)cameraPosition.Y - edge, GameEnvironment.Screen.X + edge * 2, GameEnvironment.Screen.Y + edge * 2);
        return screen.Contains(new Point((int)pos.X, (int)pos.Y));
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

    public Rectangle Screen
    {
        get { return new Rectangle((int)cameraPosition.X - edge, (int)cameraPosition.Y - edge, GameEnvironment.Screen.X + edge * 2, GameEnvironment.Screen.Y + edge * 2); }
    }

    public Vector2 SetupCamera
    {
        set { cameraPosition = value; }
    }
}