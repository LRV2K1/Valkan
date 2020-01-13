using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class MovingEntity : Entity
{
    protected string currentAnimation;
    protected int sprite_direction;
    protected double direction;
    protected double lastDirection;
    int offset;

    public MovingEntity (int boundingy, int offset, int weight = 100, int layer = 0, string id = "")
        : base(boundingy, weight, layer, id)
    {
        this.offset = offset;
        direction = 0;
        lastDirection = 1;
        sprite_direction = 3;
        currentAnimation = "A";
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        RotateAnimation();
    }

    protected void LoadAnimations(string path, string animation, int frames, bool looping = true, bool isBackAndForth = false)
    {
        int tempint = 6;
        for (int i = 0; i < 8; i++)
        {
            if (tempint > 7)
            {
                tempint = 0;
            }

            LoadAnimation(path + "/spr_" + animation + "_" + tempint + "@" + frames, animation + "_" + i, looping, isBackAndForth);
            tempint += 1;
        }
    }

    protected virtual void RotateAnimation()
    {
        int dir = sprite_direction;
        if (velocity != Vector2.Zero)
        {
            direction = Math.Atan2((double)velocity.Y, (double)velocity.X);
            if (direction < 0)
            {
                direction += 2 * Math.PI;
            }
            if (direction != lastDirection)
            {
                lastDirection = direction;
                dir = (int)((direction + (Math.PI / 8)) / (Math.PI / 4));
                if (dir > 7)
                {
                    dir = 0;
                }
                string[] anim = CurrentId.Split('_');
                sprite_direction = dir;
                PlayAnimation(anim[0] + '_' + dir);
            }
        }
    }

    public void SwitchAnimation(string animation, string currentanimation)
    {
        currentAnimation = currentanimation;
        PlayAnimation(animation + "_" + sprite_direction);
    }

    public override void PlayAnimation(string id, bool isBackWards = false)
    {
        base.PlayAnimation(id);
        origin = new Vector2(sprite.Width / 2, sprite.Height - BoundingBox.Height / 2 - offset);
    }

    public double Direction
    {
        get { return direction; }
    }
}