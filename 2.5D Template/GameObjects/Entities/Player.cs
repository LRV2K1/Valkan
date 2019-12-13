using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Player : Entity
{
    
    const float speed = 400;
    protected double direction;
    protected double lastDirection;
    protected int counter;
    protected Vector2 stillVelocity;
    protected bool selected;
    protected bool animationFinished = true;
    protected bool input = false;
    protected string currentAnimation;
    protected int health, stamina;
    protected int maxhealth, maxstamina;
    protected SkillTimer skill1, skill2, skill3;

    public Player()
        : base(60, 20, 2, "player")
    {
        maxhealth = 10;
        maxstamina = 10;
        health = maxhealth;
        stamina = maxstamina;
        
        direction = 0;
        LoadAnimation("Sprites/Player/spr_boundingbox", "boundingbox", false, false);
        int tempint = 6;
        for(int i = 0; i < 8; i++)
        {
            if(tempint > 7)
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

        skill1 = new SkillTimer("Sprites/Menu/Skills/spr_skill_0");
        skill1.Position = new Vector2(GameEnvironment.Screen.X / 2 - skill1.Width * 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        skill2 = new SkillTimer("Sprites/Menu/Skills/spr_skill_1");
        skill2.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        skill3 = new SkillTimer("Sprites/Menu/Skills/spr_skill_3");
        skill3.Position = new Vector2(GameEnvironment.Screen.X / 2 + skill1.Width * 2, GameEnvironment.Screen.Y - skill1.Width / 2);
    }
    
    public override void HandleInput(InputHelper inputHelper)
    {
        //player movement
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        if(!(overlay.CurrentOverlay is Hud))
        {
            velocity = Vector2.Zero;
            return;
        }
        if(counter > 3)
        {
            counter = 0;
            Vector2 direction = Vector2.Zero;
            if (inputHelper.IsKeyDown(Keys.A))
            {
                direction.X = -2;
            }
            else if (inputHelper.IsKeyDown(Keys.D))
            {
                direction.X = 2;
            }

            if (inputHelper.IsKeyDown(Keys.W))
            {
                direction.Y = -1;
            }
            else if (inputHelper.IsKeyDown(Keys.S))
            {
                direction.Y = 1;
            }

            float totalDir = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            if (totalDir != 0)
            {
                input = true;
                stillVelocity = new Vector2(speed * (direction.X / totalDir), speed * (direction.Y / totalDir));

                if (selected)
                {
                    Selected icon = GameWorld.GetObject("selected") as Selected;
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
                input = false;
                velocity = Vector2.Zero;
            }

            //entity selection
            if (inputHelper.KeyPressed(Keys.Z))
            {
                GameMouse mouse = GameWorld.GetObject("mouse") as GameMouse;
                string entity = mouse.CeckEntitySelected();
                if (entity != "")
                {
                    if (selected)
                    {
                        Selected icon = GameWorld.GetObject("selected") as Selected;
                        icon.SelectedEntity = entity;
                    }
                    else
                    {
                        Selected icon = new Selected(1, "selected");
                        Level level = GameWorld as Level;
                        level.RootList.Add(icon);
                        icon.SelectedEntity = entity;
                        selected = true;
                    }
                }
                else if (selected)
                {
                    Selected icon = GameWorld.GetObject("selected") as Selected;
                    Level level = GameWorld as Level;
                    level.RootList.Remove(icon.Id);
                    selected = false;
                }
            }

            //combat test
            if (inputHelper.IsKeyDown(Keys.LeftShift))
            {
                if (inputHelper.MouseLeftButtonPressed() && skill1.Ready)
                {
                    skill1.Use(3f);
                    if (selected)
                    {
                        //distance
                        Selected icon = GameWorld.GetObject("selected") as Selected;
                        Entity entity = GameWorld.GetObject(icon.SelectedEntity) as Entity;
                        float dx = entity.GlobalPosition.X - GlobalPosition.X;
                        float dy = entity.GlobalPosition.Y - GlobalPosition.Y;
                        if (Math.Sqrt(dx * dx + dy * dy) < 150)
                        {
                            RemoveSelectedEntity();
                        }
                    }
                }
            }
            else if (inputHelper.MouseLeftButtonPressed() && skill1.Ready)
            {
                skill1.Use(2f);
                if (selected)
                {
                    //distance
                    Selected icon = GameWorld.GetObject("selected") as Selected;
                    Entity entity = GameWorld.GetObject(icon.SelectedEntity) as Entity;
                    float dx = entity.GlobalPosition.X - GlobalPosition.X;
                    float dy = entity.GlobalPosition.Y - GlobalPosition.Y;
                    if (Math.Sqrt(dx * dx + dy * dy) < 100)
                    {
                        RemoveSelectedEntity();
                    }
                }
            }
            if (inputHelper.MouseRightButtonPressed() && skill2.Ready)
            {
                skill2.Use(1f);
            }
        }
        else
        {
            counter += 1;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
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

    public override void Reset()
    {
        base.Reset();
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        Overlay hud = overlay.GetOverlay("hud") as Overlay;

        hud.Add(skill1);
        hud.Add(skill2);
        hud.Add(skill3);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        this.Sprite.Color = Color.LightPink;
        base.Draw(gameTime, spriteBatch);
    }

    public override void PlayAnimation(string id, bool backWards = false)
    {
        base.PlayAnimation(id, backWards);
        origin = new Vector2(sprite.Width / 2, sprite.Height - BoundingBox.Height / 2);
    }

    private void RemoveSelectedEntity()
    {
        Selected icon = GameWorld.GetObject("selected") as Selected;
        GameWorld.GetObject(icon.SelectedEntity).RemoveSelf();
        Level level = GameWorld as Level;
        level.RootList.Remove(icon.Id);
        selected = false;
    }

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health > maxhealth)
            {
                health = maxhealth;
            }
            else if (health < 0)
            {
                health = 0;
            }
        }
    }

    public int Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            if (stamina > maxstamina)
            {
                stamina = maxstamina;
            }
            else if (stamina < 0)
            {
                stamina = 0;
            }
        }
    }

    public int MaxHealth
    {
        get { return maxhealth; }
        set
        {
            maxhealth = value;
            Health = health;
        }
    }

    public int MaxStamina
    {
        get { return maxstamina; }
        set
        {
            maxstamina = value;
            Stamina = stamina;
        }
    }
}