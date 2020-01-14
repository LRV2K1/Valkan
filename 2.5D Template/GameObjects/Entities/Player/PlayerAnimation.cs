using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

partial class Player : MovingEntity
{
    private void LoadPlayerAnimations()
    {
        LoadAnimations("Sprites/Player/" + playerType, "idle", 4, true, true);
        LoadAnimations("Sprites/Player/" + playerType, "walking", 8, true);
        LoadAnimations("Sprites/Player/" + playerType, "attack", 4, false);
        LoadAnimations("Sprites/Player/" + playerType, "die", 8, false);

        sprite_direction = 3;
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
                currentAnimation = "E";
            }
            return;
        }

        if (input)
        {
            if (currentAnimation != "C")
            {
                SwitchAnimation("walking", "C");
            }
        }
        else
        {
            if (currentAnimation != "A")
            {
                SwitchAnimation("idle", "A");
            }
        }
    }
}