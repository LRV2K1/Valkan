using System;
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

    bool die, dead;
    protected SkillTimer skill1, skill2, skill3;

    public ConnectedPlayer(string id = "player2")
        : base(0, "player")
    {
        playerid = id;
        SetStats();
        health = maxhealth;
        stamina = maxstamina;

        LoadSkills();
        SetupSkills();
    }

    protected virtual void SetStats()
    {
        maxhealth = 100;
        MaxStamina = 150;
    }

    protected virtual void LoadSkills()
    {
        skill1 = new SkillTimer("Sprites/Menu/Skills/spr_skill_0");
        skill2 = new SkillTimer("Sprites/Menu/Skills/spr_skill_4");
        skill3 = new SkillTimer("Sprites/Menu/Skills/spr_skill_5");
    }

    private void SetupSkills()
    {
        skill1.Position = new Vector2(GameEnvironment.Screen.X / 2 - skill1.Width * 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        skill2.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        skill3.Position = new Vector2(GameEnvironment.Screen.X / 2 + skill1.Width * 2, GameEnvironment.Screen.Y - skill1.Width / 2);
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
            MultiplayerManager.party.Send("Player: " + playerid + " " + "left" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.A))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "left" + " " + "false", 9999, false);
        }

        if (inputHelper.KeyPressed(Keys.D))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "right" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.D))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "right" + " " + "false", 9999, false);
        }

        if (inputHelper.KeyPressed(Keys.W))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "up" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.W))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "up" + " " + "false", 9999, false);
        }

        if (inputHelper.KeyPressed(Keys.S))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "down" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.S))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "down" + " " + "false", 9999, false);
        }

        if (inputHelper.KeyPressed(Keys.Space))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "space" + " " + "true", 9999, false);
        }
        else if (inputHelper.KeyReleased(Keys.Space))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "space" + " " + "false", 9999, false);
        }

        if (inputHelper.MouseButtonPressed(MouseButton.Left))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "leftb" + " " + "true", 9999, false);
        }
        else if (inputHelper.MouseButtonReleased(MouseButton.Left))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "leftb" + " " + "false", 9999, false);
        }

        if (inputHelper.MouseButtonPressed(MouseButton.Right))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "rightb" + " " + "true", 9999, false);
        }
        else if (inputHelper.MouseButtonReleased(MouseButton.Right))
        {
            MultiplayerManager.party.Send("Player: " + playerid + " " + "rightb" + " " + "false", 9999, false);
        }
    }

    public void GetData(string data)
    {
        Console.WriteLine(data);
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
            //GameEnvironment.AssetManager.PlaySound(die_sound);
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
                //GameEnvironment.AssetManager.PlaySound(damage_sound);
                GameEnvironment.AssetManager.PlaySound("SFX/Player/Thud");
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