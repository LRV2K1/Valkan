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

    public ConnectedPlayer(string id = "player2")
        : base(0, "player")
    {
        playerid = id;
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