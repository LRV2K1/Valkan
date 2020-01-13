using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


public class PlayingState : IGameLoopObject
{
    protected ContentManager content;
    public Level level;
    protected bool paused;
    protected bool level1;
    protected bool firstTime = true;

    public PlayingState(ContentManager content, string level)
    {
        this.content = content;
        paused = false;
        this.level = new Level(level);
        level1 = true;
    }

    //handles the playingstate
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
        firstTime = true;
        level.Reset();
        paused = false;
    }
}