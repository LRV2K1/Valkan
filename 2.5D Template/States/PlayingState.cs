using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


class PlayingState : State
{
    protected ContentManager content;
    protected Level level;
    protected bool paused;
    protected bool firstTime = true;

    public PlayingState(ContentManager content)
    {
        this.content = content;
        paused = false;
    }

    public override void Load()
    {
        string levelnum = GameEnvironment.GameSettingsManager.GetValue("level");
        level = new Level(levelnum);
    }

    public override void UnLoad()
    {
        level = null;
    }

    //handles the playingstate
    //plays the current level
    public override void HandleInput(InputHelper inputHelper)
    {
        if (level == null)
        {
            return;
        }
        if (inputHelper.KeyPressed(Keys.L))
        {
            try
            {
                switch (GameEnvironment.GameSettingsManager.GetValue("connection"))
                {
                    case "offline":
                        GameEnvironment.ScreenFade.TransitionToScene("offlineSelectionState");
                        break;
                    case "online":
                        GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState");
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                GameEnvironment.ScreenFade.TransitionToScene("modeSelectionState");
            }
            return;
        }

        if (inputHelper.KeyPressed(Keys.Escape))
        {
            OverlayManager overlay = level.GetObject("overlay") as OverlayManager;
            if (overlay.CurrentOverlay is InGameMenu)
            {
                overlay.SwitchTo("hud");
            }
            else
            {
                overlay.SwitchTo("menu");
            }
        }

        if (!paused)
        {
            level.HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (level == null)
        {
            return;
        }
        if (!paused)
        {
            level.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (level == null)
        {
            return;
        }
        level.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        firstTime = true;
        //UnLoadLevel();
        paused = false;
        //LoadLevel();
    }
}