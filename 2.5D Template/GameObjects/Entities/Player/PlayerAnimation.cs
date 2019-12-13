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
        LoadAnimation("Sprites/Player/spr_boundingbox", "boundingbox", false, false);
        int tempint = 6;
        for (int i = 0; i < 8; i++)
        {
            if (tempint > 7)
            {
                tempint = 0;
            }
            LoadAnimation("Sprites/Player/player_idle_" + tempint + "@7", "idle_" + i, true, true, 0.1f);
            LoadAnimation("Sprites/Player/player_transitionToWalkLeft_" + tempint + "@5", "idleToWalkLeft_" + i, false, false);
            LoadAnimation("Sprites/Player/player_transitionToWalkRight_" + tempint + "@5", "idleToWalkRight_" + i, false, false);
            LoadAnimation("Sprites/Player/player_walking_" + tempint + "@13", "walking_" + i, true, false);
            tempint += 1;
        }
        PlayAnimation("idle_3");
        currentAnimation = "A";
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
}