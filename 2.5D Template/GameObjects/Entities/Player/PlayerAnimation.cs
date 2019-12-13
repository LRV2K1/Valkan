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
        if ((currentAnimation == "BR" || currentAnimation == "BL") && this.Sprite.SheetIndex == this.Sprite.NumberSheetElements - 1 && !animationFinished)
        {
            animationFinished = true;
        }
        else if ((currentAnimation == "RBR" || currentAnimation == "RBL") && this.Sprite.SheetIndex == 0 & !animationFinished)
        {
            animationFinished = true;

        }
        if (stillVelocity != Vector2.Zero)
        {
            lastDirection = direction;
            direction = Math.Atan2((double)stillVelocity.Y, (double)stillVelocity.X);
            if (direction < 0)
            {
                direction += 2 * Math.PI;
            }
        }
        int dir = (int)((direction + (Math.PI / 8)) / (Math.PI / 4));
        if (dir > 7)
        {
            dir = 0;
        }
        if (input)
        {
            if (currentAnimation == "A")
            {
                //Play Animation B immediately
                PlayAnimation("idleToWalkRight_" + dir);
                animationFinished = false;

                currentAnimation = "BR";
            }
            else if (currentAnimation == "BR" && animationFinished)
            {
                //Play Animation C once animation BR is finished
                PlayAnimation("walking_" + dir);

                currentAnimation = "C";
            }
            else if (currentAnimation == "BR" && direction != lastDirection)
            {
                //Play Animation C once animation BR is finished
                int tempIndex = this.Sprite.SheetIndex;
                PlayAnimation("idleToWalkRight_" + dir);
                animationFinished = false;
                this.Sprite.SheetIndex = tempIndex;
                currentAnimation = "BR";
            }
            else if (currentAnimation == "BL" && animationFinished)
            {
                //Play Animation C once animation BL is finished
                PlayAnimation("walking_" + dir);

                this.Sprite.SheetIndex = 7;
                currentAnimation = "C";
            }
            else if (currentAnimation == "BL" && direction != lastDirection)
            {
                //Play Animation C once animation BR is finished
                int tempIndex = this.Sprite.SheetIndex;
                PlayAnimation("idleToWalkLeft_" + dir);
                animationFinished = false;
                this.Sprite.SheetIndex = tempIndex;
                currentAnimation = "BL";
            }
            else if (currentAnimation == "RBR")
            {
                int tempIndex = this.Sprite.SheetIndex;
                //Play Animation B immediately, starting with the same sheetIndex
                PlayAnimation("idle_" + dir);
                PlayAnimation("idleToWalkRight_" + dir);
                animationFinished = false;

                currentAnimation = "BR";
                this.Sprite.SheetIndex = tempIndex;
            }
            else if (currentAnimation == "RBL")
            {
                int tempIndex = this.Sprite.SheetIndex;
                //Play Animation B immediately, starting with the same sheetIndex
                PlayAnimation("idle_" + dir);
                PlayAnimation("idleToWalkLeft_" + dir);
                animationFinished = false;

                currentAnimation = "BL";
                this.Sprite.SheetIndex = tempIndex;
            }
            else if (currentAnimation == "C" && direction != lastDirection)
            {
                int tempIndex = this.Sprite.SheetIndex;
                //Keep playing animation C, changing directions accordingly
                PlayAnimation("walking_" + dir);
                currentAnimation = "C";
                this.Sprite.SheetIndex = tempIndex;
            }
        }
        else
        {
            if (currentAnimation == "BR")
            {
                int tempIndex = this.Sprite.SheetIndex;
                //Play Animation RB immediately, starting with the same sheetIndex
                PlayAnimation("idle_" + dir);
                PlayAnimation("idleToWalkRight_" + dir, true);

                currentAnimation = "RBR";
                this.Sprite.SheetIndex = tempIndex;
            }
            else if (currentAnimation == "BL")
            {
                int tempIndex = this.Sprite.SheetIndex;
                //Play Animation RB immediately, starting with the same sheetIndex
                PlayAnimation("idle_" + dir);
                PlayAnimation("idleToWalkLeft_" + dir, true);
                animationFinished = false;

                currentAnimation = "RBL";
                this.Sprite.SheetIndex = tempIndex;
            }
            else if (currentAnimation == "C")
            {
                //Play Animation RB once animation C reaches either sheetIndex 1 or 7, until then, play animation C
                if (this.Sprite.SheetIndex == 1)
                {
                    PlayAnimation("idleToWalkRight_" + dir, true); //Index 1
                    animationFinished = false;

                    currentAnimation = "RBR";
                }//Test
                else if (this.Sprite.SheetIndex == 7)
                {
                    PlayAnimation("idleToWalkLeft_" + dir, true); //Index 7
                    animationFinished = false;

                    currentAnimation = "RBL";
                }
            }
            else if ((currentAnimation == "RBR" || currentAnimation == "RBL") && animationFinished)
            {
                //Play Animation A once animation RB is finished.
                PlayAnimation("idle_" + dir);

                currentAnimation = "A";
            }
            //No condition needed for Animation A, since it will just loop itself.
        }
    }
}