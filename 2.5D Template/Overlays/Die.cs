using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


class Die : Overlay
{
    SpriteGameObject spr_die;
    Button returnButton;
    int red, green, bleu, alpha;
    float timer;
    bool done;
    public Die(GameObjectLibrary gameworld, string asset, int layer = 101, string id = "")
        : base(gameworld, layer, id)
    {
        timer = 0;
        red = 0;
        green = 0;
        bleu = 0;
        alpha = 0;
        done = false;

        spr_die = new SpriteGameObject(asset, 101);
        spr_die.Sprite.Color = new Color(red, green, bleu, alpha);
        Add(spr_die);
        returnButton = new Button("Sprites/Menu/Return_Button", 102);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        returnButton.Visible = false;
        Add(returnButton);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (timer > 0 && !done)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        timer = 0.1f;
        red+= 7;
        green+=7;
        bleu+=7;
        alpha+=7;
        if (alpha >= 255)
        {
            done = true;
            returnButton.Visible = true;
        }
        spr_die.Sprite.Color = new Color(red, green, bleu, alpha);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (returnButton.Pressed)
        {
            if (MultiplayerManager.Online)
            {
                MultiplayerManager.Party.Disconnect();
            }
            GameEnvironment.ScreenFade.TransitionToScene("titleScreen");
        }
    }
}

