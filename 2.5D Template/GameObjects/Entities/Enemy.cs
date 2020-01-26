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
    protected string target;

    public Enemy(string assetname, int boundingy, int weight = 200, int layer = 0, string id = "")
        : base(boundingy, 60, weight, layer, id)
    {
        target = "";

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
            if (startdespawntimer > 0)
            {
                startdespawntimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            despawntimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (despawntimer <= 0)
            {
                despawntimer = 0.01f;
                int R = (int)sprite.Color.R - 3;
                int G = (int)sprite.Color.G - 3;
                int B = (int)sprite.Color.B - 3;
                int A = (int)sprite.Color.A - 3;
                if (A <= 0)
                {
                    base.RemoveSelf();
                    return;
                }
                sprite.Color = new Color(R, G, B, A);
            }
            return;
        }

        if (die)
        {
            this.velocity = Vector2.Zero;
            if (Current.AnimationEnded)
            {
                dead = true;
            }
            return;
        }

        ChangeAnimation();

        if (currentAnimation == "B")
        {
            velocity = Vector2.Zero;
            return;
        }

        //MoveEnemy();
        
        if (InRange() && walking_anim) // als de player in bereik is zal de ai bewegen
        {
            Vector2 goal = (GameWorld.GetObject(target) as Entity).GridPos;
            PathFinding(goal, gameTime);
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
                GameEnvironment.AssetManager.PlayPartySound(die_sound);
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
                if (player.Dead)
                {
                    continue;
                }
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
                    GameEnvironment.AssetManager.PlayPartySound(attack_sound);
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
        List<string> players = (GameWorld as Level).PlayerList;
        float targetDistance = 100;
        if (target != "")
        {
            targetDistance = Vector2.Distance(this.GridPos, (GameWorld.GetObject(target) as Entity).GridPos);
        }
        foreach (string id in players)
        {
            Player player = GameWorld.GetObject(id) as Player;
            if (player.Dead)
            {
                continue;
            }
            float distance = Vector2.Distance(this.GridPos, player.GridPos);
            if (targetDistance > distance)
            {
                target = player.Id;
                break;
            }
        }

        return targetDistance < 15;
    }

    public override void RemoveSelf()
    {
        base.RemoveSelf();
        (GameWorld as Level).EnemyCount--;
    }

}