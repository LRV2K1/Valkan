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
    protected bool hovereffect;

    //simple button, they're active from default
    public Button(string assetname, int layer = 101, string id = "", bool hovereffect = true) :
        base(assetname, layer, id)
    {
        pressed = false;
        this.hovereffect = hovereffect;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        //If you set a button to inactive, the button will be invisible as well
        if (!visible)
        {
            return;
        }

        highLighted = BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
        pressed = inputHelper.MouseButtonPressed(MouseButton.Left) && highLighted;
        if (highLighted && hovereffect)
        {
            //The button will become slightly light red-ish to indicate the highlight.
            this.Sprite.Color = Color.LightSalmon;
        }
        else
        {
            this.Sprite.Color = Color.White;
        }
        if (pressed)
        {
            //It could play a sound here, but we've not yet made a sound ourselves. #copyright
            GameEnvironment.AssetManager.PlayPartySound("SFX/Menu/Button_Press");
        }
    }

    //This makes sure the button returns to its initial state.
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
}

