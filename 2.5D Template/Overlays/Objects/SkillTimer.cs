using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class SkillTimer : SpriteGameObject
{
    //timer for skills
    protected bool ready;
    protected float timer, waittime;
    protected SpriteGameObject overlay;

    public SkillTimer(string assetname, int layer = 101, string id = "")
        : base(assetname, layer, id)
    {
        overlay = new SpriteGameObject("Sprites/Menu/Skills/spr_skilloverlay", 102);
        overlay.Parent = this;
        origin = new Vector2(Width / 2, Height);
        overlay.Origin = origin;
        waittime = 2f;
        timer = 0;
        ready = true;
    }

    public override void Update(GameTime gameTime)
    {
        //update timer
        base.Update(gameTime);
        if (timer > 0)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else if (!ready)
        {
            ready = true;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        overlay.Sprite.Size = new Vector2(1, timer / waittime);
        overlay.Draw(gameTime, spriteBatch);
    }

    public void Use(float timer)
    {
        waittime = timer;
        this.timer = timer;
        ready = false;
    }

    public bool Ready
    {
        get { return ready; }
    }
}

