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
    
    const float speed = 400;
    protected double direction;
    protected bool selected;
    protected int health, stamina;
    protected int maxhealth, maxstamina;
    protected Skill skill1;

    public Player()
        : base(30, 20, 2, "player")
    {
        maxhealth = 10;
        maxstamina = 10;
        health = maxhealth;
        stamina = maxstamina;

        direction = 0;

        LoadAnimations();
        skill1 = new CloseAttack("Sprites/Menu/Skills/spr_skill_0");
        skill1.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2 - skill1.Timer.Width * 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
    }

    public void SetupPlayer()
    {
        skill1.Parent = this;
        skill1.Setup();
    }
    
    public override void HandleInput(InputHelper inputHelper)
    {
        Move(inputHelper);

        EntitySelection(inputHelper);

        Skills(inputHelper);
    }

    private void Move(InputHelper inputHelper)
    {
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        if (!(overlay.CurrentOverlay is Hud))
        {
            velocity = Vector2.Zero;
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
                velocity = new Vector2(-velocity.X * difirence.Y / totaldifirence - velocity.Y * difirence.X / totaldifirence, velocity.X * difirence.X / totaldifirence - velocity.Y * difirence.Y / totaldifirence);
            }
        }
        else
        {
            velocity = Vector2.Zero;
        }
    }

    public override void Update(GameTime gameTime)
    {
        ChangeAnimation();

        base.Update(gameTime);
    }

    //variabelen
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

    public bool Selected
    {
        get { return selected; }
    }
}