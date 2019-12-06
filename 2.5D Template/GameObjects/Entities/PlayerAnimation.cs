using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

partial class Player : Entity
{
    private void LoadAnimations()
    {
        LoadAnimation("Sprites/Player/spr_idle_1", "idle_1", true);
        LoadAnimation("Sprites/Player/spr_walking_1", "walking_0", true);
        LoadAnimation("Sprites/Player/spr_walking_2", "walking_1", true);
        LoadAnimation("Sprites/Player/spr_walking_3", "walking_2", true);
        LoadAnimation("Sprites/Player/spr_walking_4", "walking_3", true);
        LoadAnimation("Sprites/Player/spr_walking_5", "walking_4", true);
        LoadAnimation("Sprites/Player/spr_walking_6", "walking_5", true);
        LoadAnimation("Sprites/Player/spr_walking_7", "walking_6", true);
        LoadAnimation("Sprites/Player/spr_walking_8", "walking_7", true);
        PlayAnimation("idle_1");
    }

    private void ChangeAnimation()
    {
        if (velocity != Vector2.Zero)
        {
            direction = Math.Atan2((double)velocity.Y, (double)velocity.X);
            if (direction < 0)
            {
                direction += 2 * Math.PI;
            }
        }

        if (velocity != Vector2.Zero)
        {
            int dir = (int)((direction + (Math.PI / 8)) / (Math.PI / 4));
            if (dir > 7)
            {
                dir = 0;
            }
            PlayAnimation("walking_" + dir);
        }
        else
        {
            PlayAnimation("idle_1");
        }
    }

    public override void PlayAnimation(string id)
    {
        base.PlayAnimation(id);
        origin = new Vector2(sprite.Width / 2, sprite.Height - BoundingBox.Height / 2);
    }
}