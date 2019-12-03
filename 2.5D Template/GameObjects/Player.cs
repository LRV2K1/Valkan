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
    protected bool selected;
    protected int health, stamina;
    protected int maxhealth, maxstamina;
    bool wait;

    public Player()
        : base("Sprites/Player/spr_boundingbox", 20, 2, "player")
    {
        maxhealth = 10;
        maxstamina = 10;
        health = maxhealth;
        stamina = maxstamina;
        wait = true;

        direction = 0;

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
    
    public override void HandleInput(InputHelper inputHelper)
    {
        
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        if(!(overlay.CurrentOverlay is Hud))
        {
            velocity = Vector2.Zero;
            wait = true;
            return;
        }
        

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

            velocity = new Vector2(speed * (direction.X / totalDir), speed * (direction.Y / totalDir));

            if (selected)
            {
                Selected icon = GameWorld.GetObject("selected") as Selected;
                Vector2 difirence = icon.Position - position;
                float totaldifirence = (float)Math.Sqrt(difirence.X * difirence.X + difirence.Y * difirence.Y);
                if (totaldifirence <= 10 && velocity.Y < 0)
                {
                    velocity.Y = 0;
                }
                velocity = new Vector2(-velocity.X * difirence.Y / totaldifirence - velocity.Y * difirence.X / totaldifirence , velocity.X * difirence.X / totaldifirence - velocity.Y * difirence.Y / totaldifirence);
            }
        }
        else
        {
            velocity = Vector2.Zero;
        }

        if (inputHelper.KeyPressed(Keys.Z) && !wait)
        {
            GameMouse mouse = GameWorld.GetObject("mouse") as GameMouse;
            string entity = mouse.CeckEntitySelected();
            if (entity != "")
            {
                if (selected)
                {
                    Selected icon = GameWorld.GetObject("selected") as Selected;
                    icon.SelectedEntity = entity;
                    //icon.Position = mouse.MousePos;
                }
                else
                {
                    Selected icon = new Selected(1, "selected");
                    //icon.Position = mouse.MousePos;
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

        if (inputHelper.MouseLeftButtonPressed())
        {
            if (selected)
            {
                Selected icon = GameWorld.GetObject("selected") as Selected;
                Entity entity = GameWorld.GetObject(icon.SelectedEntity) as Entity;
                entity.RemoveSelf();
                Level level = GameWorld as Level;
                level.RootList.Remove(icon.Id);
                selected = false;
            }
        }

        wait = false;

        //test
        if (inputHelper.ScrolDown())
        {
            Stamina--;
        }
        else if (inputHelper.ScrolUp())
        {
            Stamina++;
        }

        if (inputHelper.ScrolPressed())
        {
            MaxStamina= 1;
        }
    }

    public override void Update(GameTime gameTime)
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
            int dir = (int)((direction + (Math.PI/8)) / (Math.PI/4));
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

        base.Update(gameTime);
    }

    public override void PlayAnimation(string id)
    {
        base.PlayAnimation(id);
        origin = new Vector2(sprite.Width / 2, sprite.Height - BoundingBox.Height / 2);
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