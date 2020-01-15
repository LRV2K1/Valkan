using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TextButton : TextGameObject
{
    protected bool pressed;
    public TextButton(string assetname, string text, int layer = 101, string id = "")
        : base (assetname, layer, id)
    {
        pressed = false;
        this.text = text;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (!visible)
        {
            return;
        }
        pressed = inputHelper.MouseButtonPressed(MouseButton.Left) && BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }

    public override void Reset()
    {
        base.Reset();
        pressed = false;
    }

    public bool Pressed
    {
        get { return pressed; }
    }
}