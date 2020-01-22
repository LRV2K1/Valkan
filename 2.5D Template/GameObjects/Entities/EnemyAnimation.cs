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
        input = velocity != Vector2.Zero;
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

    private void DeSpawn(GameTime gameTime)
    {
        if (startdespawntimer > 0)
        {
            startdespawntimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        despawntimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (despawntimer <= 0)
        {
            despawntimer = 0.01f;
            int R = (int)sprite.Color.R - 3;
            int G = (int)sprite.Color.G - 3;
            int B = (int)sprite.Color.B - 3;
            int A = (int)sprite.Color.A - 3;
            if (A <= 0)
            {
                base.RemoveSelf();
                return;
            }
            sprite.Color = new Color(R, G, B, A);
        }
    }

    private void Die()
    {
        this.velocity = Vector2.Zero;
        if (Current.AnimationEnded)
        {
            dead = true;
        }
    }
}