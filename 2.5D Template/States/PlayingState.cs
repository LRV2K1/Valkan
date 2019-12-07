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

    public PlayingState(ContentManager content)
    {
        this.content = content;
        level = new Level("Level_1");
    }

    public virtual void HandleInput(InputHelper inputHelper)
    {
        level.HandleInput(inputHelper);
    }

    public virtual void Update(GameTime gameTime)
    {
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

