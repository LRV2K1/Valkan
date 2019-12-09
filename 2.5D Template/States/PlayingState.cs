using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


class PlayingState : IGameLoopObject
{
    protected ContentManager content;
    protected Level level;
    protected bool firstLoad;
    ScreenFade screenFade;

    public PlayingState(ContentManager content)
    {
        this.content = content;
        level = new Level("Level_1");
        firstLoad = true;
    }

    public virtual void HandleInput(InputHelper inputHelper)
    {
        if(screenFade.FadeToWhite)
        {
            return;
        }
        level.HandleInput(inputHelper);
    }

    public virtual void Update(GameTime gameTime)
    {
        if(firstLoad)
        {
            screenFade = new ScreenFade();
            level.Add(screenFade);
            screenFade.FadeWhite();
            firstLoad = false;
        }
        level.Update(gameTime);
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        level.Draw(gameTime, spriteBatch);
    }

    public virtual void Reset()
    {
        level.Reset();
    }
}

