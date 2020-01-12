using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.IO;

public class ScreenFade : SpriteGameObject
{
    protected bool fadeToWhite;
    protected bool fadeToBlack;
    protected int r, g, b, a;
    protected int speed;
    protected string nextScene;

    public ScreenFade(string assetname, int layer = 105, string id = "screenFade") :
        base(assetname, layer, id)
    {
        this.Visible = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (fadeToBlack)
        {
            if (sprite.Color.A < 255)
            {
                a += speed;
                sprite.Color = new Color(r, g, b, a);
                return;
            }
            else
            {
                fadeToBlack = false;
                fadeToWhite = true;
                if(nextScene == "exit")
                {
                    GameEnvironment.QuitGame = true;
                    return;
                }
                GameEnvironment.GameStateManager.SwitchTo(nextScene);
            }
        }
        else if (fadeToWhite)
        {
            if (sprite.Color.A > 0)
            {
                a -= speed;
                sprite.Color = new Color(r, g, b, a);
                return;
            }
            else
            { 
                this.Visible = false;
                fadeToWhite = false;
            }
        }
    }

    public void TransitionToScene(string sceneName = "titleScreen", int newSpeed = 2)
    {
        this.Visible = true;
        speed = newSpeed;
        r = 0;
        g = 0;
        b = 0;
        a = 0;
        sprite.Color = new Color(r,g,b,a);
        this.sprite.Size = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        fadeToBlack = true;
        nextScene = sceneName;
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

