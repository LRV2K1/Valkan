using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class OutputText : TextGameObject
{
    float timer;
    public OutputText(string text) :
        base("Fonts/Hud")
    {
        this.text = text;
        color = Color.Red;
        timer = 2f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public float Timer
    {
        get { return timer; }
    }
}