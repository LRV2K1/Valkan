using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

partial class Player : MovingEntity
{
    protected bool block;
    protected bool blocked;

    protected virtual void LoadSkills()
    {
        skill1 = new CloseAttack("Sprites/Menu/Skills/spr_skill_0");
        skill2 = new Block("Sprites/Menu/Skills/spr_skill_4");
        skill3 = new Dodge("Sprites/Menu/Skills/spr_skill_5");
    }

    private void SetSkills()
    {
        if (!host)
        {
            skill1.Timer.Visible = false;
            skill2.Timer.Visible = false;
            skill3.Timer.Visible = false;
            return;
        }
        skill1.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2 - skill1.Timer.Width * 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
        skill2.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
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
        skill1.HandleInput(inputHelper);
        skill2.HandleInput(inputHelper);
        skill3.HandleInput(inputHelper);
    }

    private void RemoteSkills(InputHelper inputHelper)
    {
        //work
        if (leftb && skill1.Ready)
        {
            skill1.Use();
        }
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
        if (health <= 0 && !die)
        {
            die = true;
            SwitchAnimation("die", "D");
            GameEnvironment.AssetManager.PlaySound(die_sound);
            MediaPlayer.Stop();
            velocity = Vector2.Zero;
            (GameWorld.GetObject("overlay") as OverlayManager).SwitchTo("die");
        }
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (die)
            {
                return;
            }
            if (block && value < health)
            {
                blocked = true;
                return;
            }

            if (value < health)
            {
                GameEnvironment.AssetManager.PlaySound(damage_sound);
                GameEnvironment.AssetManager.PlaySound("SFX/Player/Thud");
            }
            health = value;
            if (MultiplayerManager.online && !host)
            {
                MultiplayerManager.party.Send("CPlayer: " + id + " health " + health, 9999, false);
            }

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
            if (MultiplayerManager.online && !host)
            {
                MultiplayerManager.party.Send("CPlayer: " + id + " stamina " + stamina, 9999, false);
            }
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
            if (MultiplayerManager.online && !host)
            {
                MultiplayerManager.party.Send("CPlayer: " + id + " maxhealth " + maxhealth, 9999, false);
            }
            Health = health;
        }
    }

    public int MaxStamina
    {
        get { return maxstamina; }
        set
        {
            maxstamina = value;
            if (MultiplayerManager.online && !host)
            {
                MultiplayerManager.party.Send("CPlayer: " + id + " maxstamina " + maxstamina, 9999, false);
            }
            Stamina = stamina;
        }
    }

    public bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }
}