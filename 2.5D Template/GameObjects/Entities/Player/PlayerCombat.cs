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
    protected bool block;
    protected bool blocked;

    protected virtual void LoadSkills()
    {
        //skill1 = new ProjectileAttack("Sprites/Menu/Skills/spr_skill_6", "Sprites/Items/Projectiles/spr_ice_", 8, "Sprites/Items/Particles/spr_ice_explosion@4");
        skill1 = new ProjectileAttack("Sprites/Menu/Skills/spr_skill_9", "Sprites/Items/Projectiles/spr_rock", 1, "Sprites/Items/Particles/spr_rock_explosion@4", 1, 5);
        //skill1 = new CloseAttack("Sprites/Menu/Skills/spr_skill_3");
        skill1.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2 - skill1.Timer.Width * 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
        skill2 = new ProjectileAttack("Sprites/Menu/Skills/spr_skill_7", "Sprites/Items/Projectiles/spr_fire_", 8, "Sprites/Items/Particles/spr_fire_explosion@3x4", 1.5f, 12, MouseButton.Right);
        //skill2 = new Block("Sprites/Menu/Skills/spr_skill_4");
        //skill2 = new BlockHold("Sprites/Menu/Skills/spr_skill_4", "Sprites/Items/Particles/spr_shield@4");
        skill2.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
        //skill3 = new Dodge("Sprites/Menu/Skills/spr_skill_5");
        skill3 = new BlockHold("Sprites/Menu/Skills/spr_skill_8", "Sprites/Items/Particles/spr_shield@4");
        skill3.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2 + skill1.Timer.Width * 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
    }

    //select entity
    private void EntitySelection(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Z))
        {
            GameMouse mouse = GameWorld.GetObject("mouse") as GameMouse;
            selected = mouse.SelectEntity();
        }
    }

    //update skills
    private void Skills(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.Q))
        {
            Health -= 3;
        }
        skill1.HandleInput(inputHelper);
        skill2.HandleInput(inputHelper);
        skill3.HandleInput(inputHelper);
    }

    private void RegenStamina(GameTime gameTime)
    {
        //check stamina timers and add stamina
        if (stamina == maxstamina)
        {
            return;
        }
        if (staminatimer >= 0)
        {
            staminatimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        if (addstaminatimer >= 0)
        {
            addstaminatimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        addstaminatimer = addstaminatimerreset;
        stamina++;
    }

    private void CheckDie()
    {
        if (health <= 0)
        {
            die = true;
            DieAnimation();
            velocity = Vector2.Zero;
        }
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (block)
            {
                blocked = true;
                return;
            }
            health = value;
            if (health > maxhealth)
            {
                health = maxhealth;
            }
            else if (health < 0)
            {
                health = 0;
            }
            CheckDie();
        }
    }

    public bool Block
    {
        get { return block; }
        set { block = value; }
    }
    public bool Blocked
    {
        get { return blocked; }
        set { blocked = value; }
    }

    public int Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            staminatimer = staminatimerreset;
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
        set { selected = value; }
    }
}