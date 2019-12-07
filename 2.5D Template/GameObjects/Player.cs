using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

partial class Player : AnimatedGameObject
{
    TextWriter textWriter;
    const float speed = 250;
    protected double direction;
    protected double lastDirection;
    protected int counter;
    protected Vector2 stillVelocity;
    protected bool selected;
    protected bool animationFinished = true;
    protected bool input = false;
    protected string currentAnimation;
    public Player()
        : base(2, "player")
    {
        direction = 0;

        LoadAnimation("Sprites/Player/spr_boundingbox", "boundingbox", false, false);
        LoadAnimation("Sprites/Player/player_idle_6@7", "idle_0", true, true);
        LoadAnimation("Sprites/Player/player_idle_7@7", "idle_1", true, true);
        LoadAnimation("Sprites/Player/player_idle_0@7", "idle_2", true, true);
        LoadAnimation("Sprites/Player/player_idle_1@7", "idle_3", true, true);
        LoadAnimation("Sprites/Player/player_idle_2@7", "idle_4", true, true);
        LoadAnimation("Sprites/Player/player_idle_3@7", "idle_5", true, true);
        LoadAnimation("Sprites/Player/player_idle_4@7", "idle_6", true, true);
        LoadAnimation("Sprites/Player/player_idle_5@7", "idle_7", true, true);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_6@5", "idleToWalkLeft_0", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_7@5", "idleToWalkLeft_1", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_0@5", "idleToWalkLeft_2", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_1@5", "idleToWalkLeft_3", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_2@5", "idleToWalkLeft_4", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_3@5", "idleToWalkLeft_5", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_4@5", "idleToWalkLeft_6", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkLeft_5@5", "idleToWalkLeft_7", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_6@5", "idleToWalkRight_0", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_7@5", "idleToWalkRight_1", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_0@5", "idleToWalkRight_2", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_1@5", "idleToWalkRight_3", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_2@5", "idleToWalkRight_4", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_3@5", "idleToWalkRight_5", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_4@5", "idleToWalkRight_6", false, false);
        LoadAnimation("Sprites/Player/player_transitionToWalkRight_5@5", "idleToWalkRight_7", false, false);
        LoadAnimation("Sprites/Player/player_walking_6@13", "walking_0", true, false);
        LoadAnimation("Sprites/Player/player_walking_7@13", "walking_1", true, false);
        LoadAnimation("Sprites/Player/player_walking_0@13", "walking_2", true, false);
        LoadAnimation("Sprites/Player/player_walking_1@13", "walking_3", true, false);
        LoadAnimation("Sprites/Player/player_walking_2@13", "walking_4", true, false);
        LoadAnimation("Sprites/Player/player_walking_3@13", "walking_5", true, false);
        LoadAnimation("Sprites/Player/player_walking_4@13", "walking_6", true, false);
        LoadAnimation("Sprites/Player/player_walking_5@13", "walking_7", true, false);
        PlayAnimation("idle_3");
        currentAnimation = "A";
        textWriter = new TextWriter("Mark", "prologue","NPC");
        
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (counter > 3)
        {
            counter = 0;
            textWriter.HandleInput(inputHelper);
            Vector2 direction = Vector2.Zero;
            if (inputHelper.IsKeyDown(Keys.A))
            {
                direction.X = -1;
            }
            else if (inputHelper.IsKeyDown(Keys.D))
            {
                direction.X = 1;
            }
            if (inputHelper.IsKeyDown(Keys.W))
            {
                direction.Y = -0.5f;
            }
            else if (inputHelper.IsKeyDown(Keys.S))
            {
                direction.Y = 0.5f;
            }

            float totalDir = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            if (totalDir != 0)
            {
                input = true;
                stillVelocity = new Vector2(speed * (direction.X / totalDir), speed * (direction.Y / totalDir));
                if (selected)
                {
                    Selected icon = GameWorld.Find("selected") as Selected;
                    Vector2 difirence = icon.Position - position;
                    float totaldifirence = (float)Math.Sqrt(difirence.X * difirence.X + difirence.Y * difirence.Y);
                    if (totaldifirence <= 10 && stillVelocity.Y < 0)
                    {
                        stillVelocity.Y = 0;
                    }
                    stillVelocity = new Vector2(-stillVelocity.X * difirence.Y / totaldifirence - stillVelocity.Y * difirence.X / totaldifirence, stillVelocity.X * difirence.X / totaldifirence - stillVelocity.Y * difirence.Y / totaldifirence);
                }
                if (currentAnimation == "C")
                {
                    velocity = stillVelocity;
                }
            }
            else
            {
                //
                input = false;
                velocity = Vector2.Zero;
            }

            if (inputHelper.MouseLeftButtonPressed())
            {
                GameMouse mouse = GameWorld.Find("mouse") as GameMouse;
                if (selected)
                {
                    Selected icon = GameWorld.Find("selected") as Selected;
                    icon.Position = mouse.Position;
                }
                else
                {
                    Selected icon = new Selected(1, "selected");
                    icon.Position = mouse.Position;
                    GameWorld.Add(icon);
                    selected = true;
                }
            }

            if (inputHelper.MouseRightButtonPressed())
            {
                if (selected)
                {
                    Selected icon = GameWorld.Find("selected") as Selected;
                    GameWorld.Remove(icon);
                    selected = false;
                }
            }
        }
        else
        {
            counter += 1;
        }
    }

    public void MovePositionOnGrid(int x, int y)
    {
        LevelGrid levelGrid = GameWorld.Find("tiles") as LevelGrid;
        position = new Vector2(x * levelGrid.CellWidth / 2 - levelGrid.CellWidth / 2 * y, y * levelGrid.CellHeight / 2 + levelGrid.CellHeight / 2 * x);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        textWriter.Update(gameTime);
        if((currentAnimation == "BR" || currentAnimation == "BL") && this.Sprite.SheetIndex == this.Sprite.NumberSheetElements - 1 && !animationFinished)
        {
            animationFinished = true;
        }
        else if((currentAnimation == "RBR" || currentAnimation == "RBL") && this.Sprite.SheetIndex == 0 & !animationFinished)
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
            else if(currentAnimation == "BR" && animationFinished)
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
            else if(currentAnimation == "RBR")
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
            else if(currentAnimation == "C" && direction != lastDirection)
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
            if(currentAnimation == "BR")
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
            else if(currentAnimation == "C")
            {
                //Play Animation RB once animation C reaches either sheetIndex 1 or 7, until then, play animation C
                if(this.Sprite.SheetIndex == 1)
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
            else if((currentAnimation == "RBR" || currentAnimation == "RBL") && animationFinished)
            {
                //Play Animation A once animation RB is finished.
                PlayAnimation("idle_" + dir);
                
                currentAnimation = "A";
            }
            //No condition needed for Animation A, since it will just loop itself.
        }
        DoPhysics();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        this.Sprite.Color = Color.LightPink;
        base.Draw(gameTime, spriteBatch);
        textWriter.Draw(gameTime, spriteBatch);
    }

    public override void PlayAnimation(string id, bool backWards = false)
    {
        base.PlayAnimation(id, backWards);
        origin = new Vector2(sprite.Width / 2, sprite.Height - GetAnimation("boundingbox").Height / 2);
    }
}
