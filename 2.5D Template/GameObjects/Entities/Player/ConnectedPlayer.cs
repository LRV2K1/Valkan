﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

class ConnectedPlayer : GameObject
{
    string playerid;

    protected int health, stamina;
    protected int maxhealth, maxstamina;
    string die_sound, damage_sound;
    protected PlayerType playerType;

    bool die, dead;
    protected SkillTimer skill1, skill2, skill3;

    public ConnectedPlayer(string id = "player2")
        : base(0, "player")
    {
        playerid = id;
        LoadStats();
        SetStats();
        LoadSkills();
        SetupPlayer();
    }

    protected virtual void LoadStats()
    {
        maxhealth = 100;
        MaxStamina = 150;
        playerType = PlayerType.Warrior;
    }

    protected void SetStats()
    {
        health = maxhealth;
        stamina = maxstamina;
    }

    protected virtual void LoadSkills()
    {
        skill1 = new SkillTimer("Sprites/Menu/Skills/spr_skill_0");
        skill2 = new SkillTimer("Sprites/Menu/Skills/spr_skill_4");
        skill3 = new SkillTimer("Sprites/Menu/Skills/spr_skill_5");
    }

    private void SetupPlayer()
    {
        skill1.Position = new Vector2(GameEnvironment.Screen.X / 2 - skill1.Width * 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        skill2.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        skill3.Position = new Vector2(GameEnvironment.Screen.X / 2 + skill1.Width * 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        /*
        string startpath = "SFX/Player/";
        if (playerType == PlayerType.Warrior)
        {
            startpath += "Warrior_";
        }
        else if (playerType == PlayerType.Wizzard)
        {
            startpath += "Mage_";
        }
        else
        {
            startpath += "Bard_";
        }
        die_sound = startpath + "Death";
        damage_sound = startpath + "Damage";
        */
    }

    public void PlayerSetup()
    {
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        Overlay hud = overlay.GetOverlay("hud") as Overlay;
        hud.Add(skill1);
        hud.Add(skill2);
        hud.Add(skill3);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.A))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "left" + " " + "true", MultiplayerManager.PartyPort, false);
        }
        else if (inputHelper.KeyReleased(Keys.A))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "left" + " " + "false", MultiplayerManager.PartyPort, false);
        }

        if (inputHelper.KeyPressed(Keys.D))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "right" + " " + "true", MultiplayerManager.PartyPort, false);
        }
        else if (inputHelper.KeyReleased(Keys.D))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "right" + " " + "false", MultiplayerManager.PartyPort, false);
        }

        if (inputHelper.KeyPressed(Keys.W))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "up" + " " + "true", MultiplayerManager.PartyPort, false);
        }
        else if (inputHelper.KeyReleased(Keys.W))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "up" + " " + "false", MultiplayerManager.PartyPort, false);
        }

        if (inputHelper.KeyPressed(Keys.S))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "down" + " " + "true", MultiplayerManager.PartyPort, false);
        }
        else if (inputHelper.KeyReleased(Keys.S))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "down" + " " + "false", MultiplayerManager.PartyPort, false);
        }

        if (inputHelper.KeyPressed(Keys.Space))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "space" + " " + "true", MultiplayerManager.PartyPort, false);
        }
        else if (inputHelper.KeyReleased(Keys.Space))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "space" + " " + "false", MultiplayerManager.PartyPort, false);
        }

        if (inputHelper.MouseButtonPressed(MouseButton.Left))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "leftb" + " " + "true", MultiplayerManager.PartyPort, false);
        }
        else if (inputHelper.MouseButtonReleased(MouseButton.Left))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "leftb" + " " + "false", MultiplayerManager.PartyPort, false);
        }

        if (inputHelper.MouseButtonPressed(MouseButton.Right))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "rightb" + " " + "true", MultiplayerManager.PartyPort, false);
        }
        else if (inputHelper.MouseButtonReleased(MouseButton.Right))
        {
            MultiplayerManager.Party.Send("Player: " + playerid + " " + "rightb" + " " + "false", MultiplayerManager.PartyPort, false);
        }
    }

    public void GetData(string data)
    {
        string[] splitdata = data.Split(' ');
        if (splitdata[1] != playerid)
        {
            return;
        }
        switch (splitdata[2])
        {
            case "health":
                Health = int.Parse(splitdata[3]);
                break;
            case "stamina":
                Stamina = int.Parse(splitdata[3]);
                break;
            case "maxhealth":
                MaxHealth = int.Parse(splitdata[3]);
                break;
            case "maxstamina":
                MaxStamina = int.Parse(splitdata[3]);
                break;
            case "skill1":
                skill1.Use(float.Parse(splitdata[3]));
                break;
            case "skill2":
                skill2.Use(float.Parse(splitdata[3]));
                break;
            case "skill3":
                skill3.Use(float.Parse(splitdata[3]));
                break;
        }
    }

    private void CheckDie()
    {
        if (health <= 0 && !die)
        {
            die = true;
            //GameEnvironment.AssetManager.PlayPartySound(die_sound);
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
            if (value < health)
            {
                //GameEnvironment.AssetManager.PlayPartySound(damage_sound);
                GameEnvironment.AssetManager.PlayPartySound("SFX/Player/Thud");
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