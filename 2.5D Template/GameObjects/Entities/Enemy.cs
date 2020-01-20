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

    protected bool input;

    //generic enemy
    //no function yet
    public Enemy(string assetname, int boundingy, int weight = 200, int layer = 0, string id = "")
        : base(boundingy, 40, weight, layer, id)
    {
        selected = false;
        dead = false;

        health = 20;
        damage = 10;
        speed = 300f;

        input = false;

        dataloc = assetname;

        LoadEnemyData();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (die || dead)
        {
            if (Current.AnimationEnded)
            {
                dead = true;
            }
            return;
        }

        ChangeAnimation();
    }

    private void CheckDie()
    {
        if (health <= 0)
        {
            die = true;
            if (die_anim)
            {
                SwitchAnimation("die", "D");
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
        }


        Vector2 nodepos = new Vector2(x, y);
        nodeStart = new Node(this.GridPos, Vector2.Distance(this.GridPos, playerpos));
        nodeEnd = new Node(playerpos, 0);
        if (nodepos == playerpos)
        {
            nodes[x, y] = nodeEnd;
        }
        else if (nodepos == this.GridPos)
        {
            nodes[x, y] = nodeStart;
        }
        else
        {
            nodes[x, y] = new Node(nodepos, hcost_grid[x, y]);//node wordt toegovoegd aan de lijst van nodes en de hcost wordt toegevoegt aan de Node
            if (grid.GetTileType(x, y) == TileType.Wall)
            {
                nodes[x, y].obstacle = true;//wanneer in de grid van de map een muur staat zal de hcost grid die tellen als een onbruikbare getal
            }
        }
        foreach (Node node in nodes)
        {
            if (node.nodeXY == nodepos)
            {
                node.fGlobalGoal = hcost_grid[x, y];
            }
        }
    }


    private void CalculateNeighbours(Node currentNode)
    {
        foreach (Node n in nodes)
        {
            //De if-statements hieronder voegen de posities rondom de aipos aan de OpenNodesList
            if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X + 1, (int)currentNode.nodeXY.Y + 1))
            {
                currentNode.neighbours.Add(n);
            }
            else if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X - 1, (int)currentNode.nodeXY.Y - 1))
            {
                currentNode.neighbours.Add(n);
            }
            else if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X + 1, (int)currentNode.nodeXY.Y - 1))
            {
                currentNode.neighbours.Add(n);
            }
            else if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X - 1, (int)currentNode.nodeXY.Y + 1))
            {
                currentNode.neighbours.Add(n);
            }
            else if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X + 1, (int)currentNode.nodeXY.Y))
            {
                currentNode.neighbours.Add(n);
            }
            else if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X, (int)currentNode.nodeXY.Y + 1))
            {
                currentNode.neighbours.Add(n);
            }
            else if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X - 1, (int)currentNode.nodeXY.Y))
            {
                currentNode.neighbours.Add(n);
            }
            else if (n.nodeXY == new Vector2((int)currentNode.nodeXY.X, (int)currentNode.nodeXY.Y - 1))
            {
                currentNode.neighbours.Add(n);
            }
        }
    }
    /// <summary>
    /// //////////////////////9999
    /// </summary>
    /// <param name="pos"></param>
    private void Move(Vector2 pos)
    {
        Enemy enemy = this;
        LevelGrid grid = GameWorld.GetObject("levelgrid") as LevelGrid;
        Player player = GameWorld.GetObject("player") as Player;
        Vector2 movpos = grid.AnchorPosition((int)pos.X, (int)pos.Y);
        //  Vector2 enemypos = new Vector2((int)this.GridPos.X, (int)this.GridPos.Y);
        //  Vector2 endpos = new Vector2((int)nodeEnd.nodeXY.X,(int)nodeEnd.nodeXY.Y);
        //de ai beweegt naar de gewezen positie
        float dx = movpos.X - this.Position.X;
        float dy = movpos.Y - this.Position.Y;
        float distance = Vector2.Distance(movpos, this.Position);
        float scale = 100 / distance;

        float aiplayerdistance = Vector2.Distance(this.GridPos, player.GridPos);

        if (aiplayerdistance < 2.2f || die || dead)
            this.velocity = Vector2.Zero;
        else
        {
            this.velocity.X = dx * scale;
            this.velocity.Y = dy * scale;
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
}