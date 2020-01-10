using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

class Button : SpriteGameObject
{
    protected bool pressed;
    protected bool highLighted;
    protected bool active;

    //simple button
    public Button(string assetname, int layer = 101, string id = "") :
        base(assetname, layer, id)
    {
        pressed = false;
        active = false;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if(!active)
        {
            this.Visible = false;
            return;
        }
        else
        {
            this.Visible = true;
        }
        highLighted = BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
        pressed = inputHelper.MouseButtonPressed(MouseButton.Left) && highLighted;
        if (highLighted)
        {
            this.Sprite.Color = Color.Black;
        }
        else
        {
            this.Sprite.Color = Color.White;
        }
        if (pressed)
        {
            //GameEnvironment.AssetManager.PlaySound("Sounds/snd_button_select");
        }
    }

    public override void Reset()
    {
        base.Reset();
        pressed = false;
        highLighted = false;
    }

    public bool Pressed
    {
        get { return pressed; }
    }

    public bool HighLighted
    {
        get { return highLighted; }
    }

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }
}

