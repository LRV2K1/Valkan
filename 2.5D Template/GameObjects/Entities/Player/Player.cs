using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Graphics;

struct SpeedMultiplier
{
    public float multiplier;
    public float time { get; set; }
    public SpeedMultiplier(float m, float t)
    {
        multiplier = m;
        time = t;
    }
}

enum PlayerType
{
    Warrior,
    Bard,
    Wizzard,
}

partial class Player : MovingEntity
{
    protected bool gamehost;
    protected float speed = 400;
    protected bool selected;
    protected int health, stamina;
    protected int maxhealth, maxstamina;
    protected string name;
    protected int playerlevel, playerEXP, EXPThreshold;
    public int playerID;
    protected Skill skill1, skill3;
    protected Skill skill2;
    protected float staminatimer, staminatimerreset, addstaminatimer, addstaminatimerreset;
    protected bool dead, die;
    string die_sound, damage_sound;
    protected bool input;
    protected PlayerType playerType;
    protected bool inmovible;

    protected bool up, down, left, right, leftb, rightb, space;

    protected List<SpeedMultiplier> speedMultipliers;

    private static string hash = "_+*/(&!*";
    public static string EncryptedText;

    Vector2 inputDirection;

    public Player(bool host = true, string id = "player")
        : base(30, 58, 20, 2, id)
    {
        inmovible = false;

        this.gamehost = host;
        name = "Valkan";
        playerID = 1;
        playerlevel = 1;
        playerEXP = 1;

        EXPThreshold = 5;

        dead = false;
        die = false;

        speedMultipliers = new List<SpeedMultiplier>();

        LoadStats();
        SetStats();

        LoadPlayerAnimations();

        LoadSkills();
        SetSkills();
    }

    public void GetData(string data)
    {
        Console.WriteLine(data);
        string[] splitdata = data.Split(' ');
        switch (splitdata[2])
        {
            case "left":
                left = bool.Parse(splitdata[3]);
                break;
            case "right":
                right = bool.Parse(splitdata[3]);
                break;
            case "up":
                up = bool.Parse(splitdata[3]);
                break;
            case "down":
                down = bool.Parse(splitdata[3]);
                break;
            case "leftb":
                leftb = bool.Parse(splitdata[3]);
                break;
            case "rightb":
                rightb = bool.Parse(splitdata[3]);
                break;
            case "space":
                space = bool.Parse(splitdata[3]);
                break;
        }
    }

    protected virtual void LoadStats()
    {
        playerType = PlayerType.Warrior;
        maxhealth = 10;
        maxstamina = 100;
        staminatimerreset = 1f;
        addstaminatimerreset = 0.02f;
    }

    private void SetStats()
    {
        health = maxhealth;
        stamina = maxstamina;
    }

    //setup skills
    public void SetupPlayer()
    {
        skill1.Parent = this;
        skill1.Setup();
        skill2.Parent = this;
        skill2.Setup();
        skill3.Parent = this;
        skill3.Setup();
        string startpath = "SFX/Player/";
        if(playerType == PlayerType.Warrior)
        {
            startpath += "Warrior_";
        }
        else if(playerType == PlayerType.Wizzard)
        {
            startpath += "Mage_";
        }
        else
        {
            startpath += "Bard_";
        }
        die_sound = startpath + "Death";
        damage_sound = startpath + "Damage";
    }
    
    public override void HandleInput(InputHelper inputHelper)
    {
        if (die || dead)
        {
            return;
        }
        if (gamehost)
        {
            ControlMove(inputHelper);

            EntitySelection(inputHelper);

            Skills(inputHelper);
        }
        else
        {
            RemoteControlMove(inputHelper);
            RemoteSkills(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        //update loop
        if (!die && !dead)
        {
            Move(gameTime);

            ChangeAnimation();

            RegenStamina(gameTime);
        }
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        skill1.Draw(gameTime, spriteBatch);
    }

    private void ControlMove(InputHelper inputHelper)
    {
        inputDirection = Vector2.Zero;
        if (inmovible)
        {
            input = false;
            return;
        }

        if (inputHelper.IsKeyDown(Keys.A))
        {
            inputDirection.X = -2;
        }
        else if (inputHelper.IsKeyDown(Keys.D))
        {
            inputDirection.X = 2;
        }

        if (inputHelper.IsKeyDown(Keys.W))
        {
            inputDirection.Y = -1;
        }
        else if (inputHelper.IsKeyDown(Keys.S))
        {
            inputDirection.Y = 1;
        }
    }

    private void RemoteControlMove(InputHelper inputHelper)
    {
        inputDirection = Vector2.Zero;
        if (inmovible)
        {
            input = false;
            return;
        }

        if (left)
        {
            inputDirection.X = -2;
        }
        else if (right)
        {
            inputDirection.X = 2;
        }

        if (up)
        {
            inputDirection.Y = -1;
        }
        else if (down)
        {
            inputDirection.Y = 1;
        }
    }

    private void Move(GameTime gameTime)
    {
        OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
        if (!(overlay.CurrentOverlay is Hud))
        {
            velocity = Vector2.Zero;
            return;
        }

        //check direction and movement
        float totalDir = (float)Math.Sqrt(inputDirection.X * inputDirection.X + inputDirection.Y * inputDirection.Y);
        if (totalDir != 0)
        {
            input = true;
            Vector2 stillVelocity = new Vector2(speed * (inputDirection.X / totalDir), speed * (inputDirection.Y / totalDir));

            //change movement if selected
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
            else
            {
                velocity = Vector2.Zero;
            }
        }
        else
        {
            input = false;
            velocity = Vector2.Zero;
        }

        Multipliers(gameTime);
    }

    private void Multipliers(GameTime gameTime)
    {
        for (int i = speedMultipliers.Count - 1; i >= 0; i--)
        {
            SpeedMultiplier s = speedMultipliers[i];
            velocity *= s.multiplier;
            s.time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (s.time < 0)
            {
                speedMultipliers.RemoveAt(i);
            }
            else
            {
                speedMultipliers.RemoveAt(i);
                speedMultipliers.Add(s);
            }
        }
    }

    public void AddSpeedMultiplier(float time, float multiplier)
    {
        speedMultipliers.Add(new SpeedMultiplier(multiplier, time));
    }

    public void LevelUp()
    {
        if (playerEXP > EXPThreshold)
        {
            playerlevel++;
            playerEXP = playerEXP - EXPThreshold;
            EXPThreshold = EXPThreshold * 2;
        }
        WriteStats();
    }

    public bool InMovible
    {
        get { return inmovible; }
        set
        {
            inmovible = value;
            if (inmovible)
            {
                velocity = Vector2.Zero;
            }
        }
    }


    //obsolete
    public virtual void WriteStats()                        //Saves the player stats in a text file
    {
        string statpath = "Content/PlayerStats/Stats.txt";
        string[] lines;
        lines = new string[7];
        StreamWriter writer = new StreamWriter(statpath);
        lines[0] = Encrypt(name);
        lines[1] = Encrypt(playerType.ToString());
        lines[2] = Encrypt(playerID.ToString());
        lines[3] = Encrypt(playerlevel.ToString());
        lines[4] = Encrypt(playerEXP.ToString());
        lines[5] = Encrypt(maxhealth.ToString());
        lines[6] = Encrypt(maxstamina.ToString());
        for (int i = 0; i < lines.Length; i++)
        {
            writer.WriteLine(lines[i]);
        }
        writer.Close();
    }

    public void ReadStats()                                //Reads the player stats in a text file
    {
        string statspath = "Content/PlayerStats/Stats.txt";
        StreamReader streamReader = new StreamReader(statspath);
        List<string> lines = new List<string>();
        string line = streamReader.ReadLine();
        while (line != null)
        {
            lines.Add(line);
            line = streamReader.ReadLine();
        }
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i] = Decrypt(lines[i]);
        }
        streamReader.Close();
    }

    public static string Encrypt(string input)
    {
        byte[] file = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(file, 0, file.Length);
                EncryptedText = Convert.ToBase64String(results, 0, results.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] file = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(file, 0, file.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }

    public bool Host
    {
        get { return gamehost; }
    }
}