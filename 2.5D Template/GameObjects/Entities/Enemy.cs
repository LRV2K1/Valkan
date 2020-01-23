using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

partial class Enemy : MovingEntity
{
    protected int health, damage;
    protected float speed;
    protected bool die, dead;
    protected bool selected;
    protected string dataloc;
    protected float attacktimer;
    protected float resetattacktimer;

    protected float despawntimer;
    protected float startdespawntimer = 10;

    protected bool input;

    public Enemy(string assetname, int boundingy, int weight = 200, int layer = 0, string id = "")
        : base(boundingy, 40, weight, layer, id)
    {
        selected = false;
        dead = false;
        despawntimer = 0;

        health = 20;
        damage = 10;
        speed = 300f;
        resetattacktimer = 1.5f;
        attacktimer = 0;

        input = false;

        dataloc = assetname;

        LoadEnemyData();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (dead)
        {
            DeSpawn(gameTime);
            return;
        }

        if (die)
        {
            Die();
            return;
        }

        ChangeAnimation();

        if (currentAnimation == "B")
        {
            velocity = Vector2.Zero;
            return;
        }
        
        if (InRange()) // als de player in bereik is zal de ai bewegen
        {
            //MoveEnemy();
            Player player = GameWorld.GetObject("player") as Player;
            PathFinding(player.GridPos, gameTime);
            MovePath(gameTime);
        }
        else if (!InRange())
        {
            this.velocity = Vector2.Zero;
        }
        
        Attack(gameTime);
    }

    private void CheckDie()
    {
        if (health <= 0)
        {
            die = true;
            if (die_anim)
            {
                SwitchAnimation("die", "D");
                velocity = Vector2.Zero;
                GameEnvironment.AssetManager.PlaySound(die_sound);
            }
            else
            {
                RemoveSelf();
            }
            if (selected)
            {
                GameMouse mouse = GameWorld.GetObject("mouse") as GameMouse;
                mouse.RemoveSelectedEntity();
            }
            (GameWorld as Level).EnemyCount--;
        }
    }

    private void MoveEnemy()
    {
        Player player = GameWorld.GetObject("player") as Player;
        float distance = Vector2.Distance(player.Position, position);
        float dx = player.Position.X - position.X;
        float dy = player.Position.Y - position.Y;
        velocity = new Vector2((dx / distance) * speed, (dy / distance) * speed);
    }

    private void Attack(GameTime gameTime)
    {
        if (!attack_anim && damage > 0)
        {
            return;
        }
        if (attacktimer > 0)
        {
            attacktimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        bool attacked = false;
        Rectangle rectangle = new Rectangle((int)GlobalPosition.X - 30, (int)GlobalPosition.Y - 30, 60, 60);
        List<string> surroudningEnities = GetSurroundingEntities();
        foreach (string id in surroudningEnities)
        {
            Player player = GameWorld.GetObject(id) as Player;
            if (player != null)
            {
                if (attacked)
                {
                    if (rectangle.Intersects(player.BoundingBox))
                    {
                        player.Health -= damage;
                        continue;
                    }
                }
                float distance = Vector2.Distance(player.Position, position);
                Vector2 directions = new Vector2(Math.Sign(player.Position.X - position.X), Math.Sign(player.Position.Y - position.Y));
                if (distance < 100)
                {
                    attacktimer = resetattacktimer;
                    SwitchAnimation("attack", "B");
                    GameEnvironment.AssetManager.PlaySound(attack_sound);
                    velocity = directions;
                    attacked = true;
                    Vector2 range = new Vector2(50 * (float)Math.Cos(Direction), 50 * (float)Math.Sin(Direction));
                    rectangle.X += (int)range.X;
                    rectangle.Y += (int)range.Y;
                    if (rectangle.Intersects(player.BoundingBox))
                    {
                        player.Health -= damage;
                    }
                    continue;
                }
            }
        }
    }

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            CheckDie();
        }
    }

    public bool Dead
    {
        get { return die; }
    }

    public bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }

    bool InRange()
    {
        bool range = false;
        Player player = GameWorld.GetObject("player") as Player;
        float distance = Vector2.Distance(gridpos, player.GridPos);

        if (distance < 15)
            range = true;

        return range;
    }

    public override void RemoveSelf()
    {
        base.RemoveSelf();
        (GameWorld as Level).EnemyCount--;
    }

}

