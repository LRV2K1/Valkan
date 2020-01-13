using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

partial class Enemy : MovingEntity
{

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
            if (currentAnimation != "C" && walking_anim)
            {
                SwitchAnimation("walking", "C");
            }
        }
        else
        {
            if (currentAnimation != "A" && idle_anim)
            {
                SwitchAnimation("idle", "A");
            }
        }
    }
}