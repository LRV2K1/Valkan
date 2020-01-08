using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


class PlayingState : IGameLoopObject
{
    protected ContentManager content;
    public Level level;
    protected bool paused;
    protected bool level1;

    public PlayingState(ContentManager content)
    {
        this.content = content;
        paused = false;
        level = new Level("Level_1");
        level1 = true;
    }

    //handels the payingstate
    //plays the current level
    public virtual void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.L))
        {
            if (level1)
            {
                level = null;
                level = new Level("Level_2");
            }
            else
            {
                level = null;
                level = new Level("Level_1");
            }
            level1 = !level1;
        }

        if (inputHelper.KeyPressed(Keys.P))
        {
            paused = !paused;
        }

        if (!paused)
        {
            level.HandleInput(inputHelper);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        if (!paused)
        {
            level.Update(gameTime);
        }
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        level.Draw(gameTime, spriteBatch);
    }

    public virtual void Reset()
    {
        level.Reset();
        paused = false;
    }
}