using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

partial class Player : Entity
{

    private void LoadAnimations()
    {
        int tempint = 6;
        for (int i = 0; i < 8; i++)
        {
            if (tempint > 7)
            {
                tempint = 0;
            }
            
            LoadAnimation("Sprites/Player/"+ playerType + "/spr_idle_" + tempint + "@4", "idle_" + i, true, true);
            LoadAnimation("Sprites/Player/" + playerType + "/spr_walking_" + tempint + "@8", "walking_" + i, true, false);
            LoadAnimation("Sprites/Player/" + playerType + "/spr_attack_" + tempint + "@4", "attack_" + i, false, false);
            LoadAnimation("Sprites/Player/" + playerType + "/spr_die_" + tempint + "@8", "die_" + i, false, false);
            tempint += 1;
        }
        PlayAnimation("idle_3");
        currentAnimation = "A";
    }

    private void ChangeAnimation()
    {
        //check if attacking
        if (currentAnimation == "B")
        {
            if (Current.AnimationEnded)
            {
                //when attack finished
                currentAnimation = "D";
            }
            return;
        }


        int dir = int.Parse(CurrentId.Split('_')[1]);
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
                PlayAnimation(anim[0] + '_' + dir);
            }
        }

        if (input)
        {
            if (currentAnimation != "C")
            {
                currentAnimation = "C";
                PlayAnimation("walking_" + dir);
            }
        }
        else
        {
            if (currentAnimation != "A")
            {
                currentAnimation = "A";
                PlayAnimation("idle_" + dir);
            }
        }
    }

    public void AttackAnimation()
    {
        currentAnimation = "B";
        int dir = int.Parse(CurrentId.Split('_')[1]);
        PlayAnimation("attack_" + dir);
    }

    private void DieAnimation()
    {
        int dir = int.Parse(CurrentId.Split('_')[1]);
        PlayAnimation("die_" + dir);
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