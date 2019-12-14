using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

public class ScreenFade : SpriteGameObject
{
    protected bool fadeToWhite;
    protected bool fadeToBlack;
    protected int r, g, b, a;
    protected int speed;

    public ScreenFade(string assetname = "Sprites/Menu/spr_button", int layer = 105, string id = "screenFade") :
        base(assetname, layer, id)
    {

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (fadeToBlack)
        {
            if (sprite.Color.A < 255)
            {
                r += speed;
                g += speed;
                b += speed;
                a += speed;
                sprite.Color = new Color(r, g, b, a);
                return;
            }
            fadeToBlack = false;
        }
        else if (fadeToWhite)
        {
            if (sprite.Color.A > 0)
            {
                r -= speed;
                g -= speed;
                b -= speed;
                a -= speed;
                sprite.Color = new Color(r, g, b, a);
                return;
            }
            this.Visible = false;
            fadeToWhite = false;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        //If fade Black
        //Draw previous scene
        //Draw black fade

        //If fade White
        //Draw current scene
        //Draw white fade
        //When finished, currentscene is currentscene -> gamestatemanager

        base.Draw(gameTime, spriteBatch);
    }

    public void FadeWhite()
    {
        this.Sprite.Color = Color.Black;
        this.Sprite.Size = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        this.Visible = true;
        fadeToWhite = true;
        speed = 2;
        r = 255;
        g = 255;
        b = 255;
        a = 255;
        sprite.Color = new Color(r, g, b, a);
    }
    public void FadeBlack()
    {
        this.Visible = true;
        fadeToBlack = true;
        speed = 2;
        r = 0;
        g = 0;
        b = 0;
        a = 0;
        sprite.Color = new Color(r, g, b, a);
    }

    public bool FadeToWhite
    {
        get { return fadeToWhite; }
    }

    public bool FadeToBlack
    {
        get { return fadeToBlack; }
    }

    public int A
    {
        get { return a; }
    }
}

