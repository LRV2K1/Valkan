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
        //When the screenfade is initiated, the opacity will increase from 0 to 255, this speed can be altered with the speed variable.
        if (fadeToBlack)
        {
            if (sprite.Color.A < 255)
            {
                a += speed;
                sprite.Color = new Color(0,0,0, a);
                return;
            }
            else
            {
                //If the opacity 255 is met, the screenfade will make sure the next state is loaded and it will fade back to 0 opacity at the same time.
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
        //The screenfade goes from opacity 255 to 0
        else if (fadeToWhite)
        {
            if (sprite.Color.A > 0)
            {
                a -= speed;
                sprite.Color = new Color(0,0,0, a);
                return;
            }
            else
            { 
                //When 0 is met, the screenfade will be invisible again.
                this.Visible = false;
                fadeToWhite = false;
            }
        }
    }

    //This method starts a buffer between states, you can alter which state it goes to and the speed of the fade.
    public void TransitionToScene(string sceneName = "titleScreen", int newSpeed = 2)
    {
        this.Visible = true;
        speed = newSpeed;
        a = 0;
        sprite.Color = new Color(0,0,0,a);
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

