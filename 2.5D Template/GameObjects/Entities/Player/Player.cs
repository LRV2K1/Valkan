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

partial class Player : Entity
{
    const float speed = 400;
    protected double direction;
    protected bool selected;
    protected int health, stamina;
    protected int maxhealth, maxstamina;
    protected string name, job;
    protected int playerlevel;
    public static int playerEXP, EXPThreshold;
    public int playerID;
    protected Skill skill1, skill3;
    protected Block skill2;
    protected float staminatimer, staminatimerreset, addstaminatimer, addstaminatimerreset;
    protected bool dead, die;
    protected string currentAnimation;
    protected bool animationFinished = false;
    protected double lastDirection;
    protected bool input;
    protected Vector2 stillVelocity;

    protected List<SpeedMultiplier> speedMultipliers;

    private static string hash = "_+*/(&!*";
    public static string EncryptedText;

    public Player()
        : base(30, 20, 2, "player")
    {
        name = "Valkan";
        job = "Light";
        playerID = 1;
        playerlevel = 1;
        playerEXP = 0;
        maxhealth = 10;
        maxstamina = 100;
        health = maxhealth;
        stamina = maxstamina;
        staminatimerreset = 1f;
        addstaminatimerreset = 0.02f;
        EXPThreshold = 5;

        dead = false;
        die = false;

        speedMultipliers = new List<SpeedMultiplier>();

        direction = 0;

        LoadAnimations();
        skill1 = new CloseAttack("Sprites/Menu/Skills/spr_skill_0");
        skill1.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2 - skill1.Timer.Width * 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
        skill2 = new Block("Sprites/Menu/Skills/spr_skill_4");
        skill2.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
        skill3 = new Dodge("Sprites/Menu/Skills/spr_skill_5");
        skill3.Timer.Position = new Vector2(GameEnvironment.Screen.X / 2 + skill1.Timer.Width * 2, GameEnvironment.Screen.Y - skill1.Timer.Width / 2);
    }

    public void SetupPlayer()
    {
        skill1.Parent = this;
        skill1.Setup();
        skill2.Parent = this;
        skill2.Setup();
        skill3.Parent = this;
        skill3.Setup();
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (die || dead)
        {
            return;
        }
        ControlMove(inputHelper);

        EntitySelection(inputHelper);

        Skills(inputHelper);
    }

    public override void Update(GameTime gameTime)
    {
        if (!die && !dead)
        {
            Move(gameTime);

            ChangeAnimation();

            RegenStamina(gameTime);
        }
        base.Update(gameTime);
    }

    private void ControlMove(InputHelper inputHelper)
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
        if (inputHelper.KeyPressed(Keys.Q))
        {
            ReadStats();
            WriteStats();
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

    }

    private void Move(GameTime gameTime)
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
        playerlevel++;
        playerEXP = playerEXP - EXPThreshold;
        EXPThreshold = EXPThreshold * 2;
        WriteStats();
    }

    public virtual void WriteStats()                        //Saves the player stats in a text file
    {
        string statpath = "Content/PlayerStats/Stats.txt";
        string[] lines;
        lines = new string[7];
        StreamWriter writer = new StreamWriter(statpath);
        lines[0] = Encrypt(name);
        lines[1] = Encrypt(job);
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
        name = Decrypt(lines[0]);
        job = Decrypt(lines[1]);
        playerID = Int32.Parse(Decrypt(lines[2]));
        playerlevel = Int32.Parse(Decrypt(lines[3]));
        playerEXP = Int32.Parse(Decrypt(lines[4]));
        maxhealth = Int32.Parse(Decrypt(lines[5]));
        maxstamina = Int32.Parse(Decrypt(lines[6]));
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i] = Decrypt(lines[i]);
            System.Diagnostics.Debug.WriteLine(lines[i]);
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
}