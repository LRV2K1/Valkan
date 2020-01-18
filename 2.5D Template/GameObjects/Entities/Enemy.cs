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

    int count = 0;

    float[,] hcost_grid = new float[30, 30];
    bool pathFound = false;
    // nieuw path word aangemaakt
    enum AiState { SLEEP, RUNNING } // de ai heeft 2 states waarin hij switched
    AiState currentState = AiState.SLEEP;
    Node nodeStart;
    Node nodeEnd;
    List<Node> path = new List<Node>();
    Node[,] nodes = new Node[30, 30];
    List<Vector2> destinationQueue = new List<Vector2>();
    List<Node> untestedNodesList = new List<Node>();
    int counter;
    int start = 1;

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

        if (start == 1) // de start positie moet 1 keer worden geintialized
        {
            Startup();
            start = 0;
        }
        if (InRange() == true) // als de player in bereik is zal de ai bewegen
        {
            Player player = GameWorld.GetObject("player") as Player;
            // this.Position *= EnemyVelocity(player.Position);
            DesCalculate(player.GridPos);
        }
        else if (!InRange()|| die || dead)
            this.velocity = Vector2.Zero;

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
    }
    public void Startup()
    {
        Enemy enemy = this;
        Player player = GameWorld.GetObject("player") as Player;
        destinationQueue.Add(player.GridPos); //De StartPositie wordt toegevoegd aan de destinationQueue
        LevelGrid grid = GameWorld.GetObject("levelgrid") as LevelGrid;

        for (int y = 0; y < 30; y++)
        {
            for (int x = 0; x < 30; x++)
            {
                hcost_grid[x, y] = (float)Vector2.Distance(new Vector2(x, y), player.GridPos); //de hcostgrid krijgt elk vakje een value, de value is de afstand vanaf het vakje naar de player (destination).

                Vector2 nodepos = new Vector2(x, y);
                nodeStart = new Node(this.GridPos, Vector2.Distance(this.GridPos, player.GridPos));
                nodeEnd = new Node(player.GridPos, 0);
                if (nodepos == player.GridPos)
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
            }
        }
    }
    public void DesCalculate(Vector2 playerpos)
    {
        Enemy enemy = this;
        count++;
        counter++;
        if (destinationQueue.Count() >= 1) //als er een distination  in de lijst staat zal de ai naar daar gaan
        {
            float distance = Vector2.Distance(playerpos, this.GridPos);
            if (distance < 2 && pathFound) //de ai heeft de player gevonden door de path te gebruiken
            {
                //found 
                currentState = AiState.SLEEP;
                destinationQueue.RemoveAt(0);  //de laatste destination wordt verwijderd
                pathFound = false;
                path.Clear();
                count = 0;
            }

            else if (distance < 2 && !pathFound)  //de ai heeft de player gevonden maar zonder de path te gebruiken, dus de player kwam naar de ai
            {
                //found 
                currentState = AiState.SLEEP;
                pathFound = false;
                path.Clear();
                destinationQueue.RemoveAt(0);  //de laatste destination wordt verwijderd
                count = 0;
            }
            else if (distance > 2 && !pathFound) //de ai moet de path naar de player vinden 
            {
                //new
                currentState = AiState.SLEEP;
            }
            else if (distance > 2 && pathFound) //de ai heeft de path gevonden maar de player nog niet
            {
                //busy
                currentState = AiState.RUNNING;
                count = 0;
            }
        }
        switch (currentState)
        {
            case AiState.SLEEP:
                if (count >= 10)//de ai heeft een delay van 1 sec
                {
                    destinationQueue.Add(playerpos); //de gegeven playerpostitie wordt toegevoegd aan de destinationQueue
                    FindPath(destinationQueue[0]); //Findpath word aanroepen om de pad te vinden van de destination dat vooraan de lijst staat
                    Console.WriteLine("Gridpos :  " + playerpos);
                }
                break;

            case AiState.RUNNING:
                if (path.Count() != 0) //wanneer de path berekend
                {
                    if (counter > 20)
                    {
                        Move(path[path.Count() - 1].nodeXY); //beweeg naar de eerste positie in de list
                        path.RemoveAt(path.Count() - 1); // verwijder de positie na het bewegen
                        counter = 0;
                    }

                }
                else
                {
                    pathFound = false;
                    destinationQueue.RemoveAt(0);
                    currentState = AiState.SLEEP;
                }
                break;
        }

    }
    public void FindPath(Vector2 playerpos)
    {
        Enemy enemy = this;

        CalculateHcost(playerpos); //de Hcost wordt berekend wanneer de player een nieuwe positie heeft

        Node nodeCurrent = nodeStart;
        nodeStart.fLocalGoal = 0.0f;
        untestedNodesList.Add(nodeStart);

        while (untestedNodesList.Count() > 0 && nodeCurrent.nodeXY != new Vector2((int)nodeEnd.nodeXY.X, (int)nodeEnd.nodeXY.Y)) //zolang er waardes in de lijst zijn
        {
            untestedNodesList.OrderBy(n => n.fGlobalGoal).ToList();         // order by laag naar hoog
            while (untestedNodesList.Count() != 0 && untestedNodesList[0].bvisited)       //zolang te testen nodes niet leeg is en de node met laagste cost visited is
            {
                untestedNodesList.RemoveAt(0);
            }
            if (untestedNodesList.Count() == 0)
            {
                Console.WriteLine("LIST IS 0 :)");
                break;
            }
            nodeCurrent = untestedNodesList[0];
            nodeCurrent.bvisited = true;

            CalculateNeighbours(nodeCurrent);
            foreach (Node neighbour in nodeCurrent.neighbours)
            {
                if (!neighbour.bvisited && !neighbour.obstacle)
                    untestedNodesList.Add(neighbour);
                float possiblyLowerGoal = nodeCurrent.fLocalGoal + Vector2.Distance(nodeCurrent.nodeXY, neighbour.nodeXY);
                if (possiblyLowerGoal < neighbour.fLocalGoal)
                {
                    neighbour.parent = nodeCurrent;
                    neighbour.fLocalGoal = possiblyLowerGoal;
                    neighbour.fGlobalGoal = neighbour.fLocalGoal + Vector2.Distance(neighbour.nodeXY, nodeEnd.nodeXY);
                }
            }
            Console.WriteLine("|" + (nodeCurrent.nodeXY == nodeEnd.nodeXY) + "|" + nodeCurrent.nodeXY + "|" + nodeEnd.nodeXY + "|");
        }

        pathFound = true;
        untestedNodesList.Clear();
        AddParentToPath(nodeCurrent);

    }

    public void AddParentToPath(Node n)
    {
        if (n.parent != null)
        {
            path.Add(n.parent);
            AddParentToPath(n.parent);
        }
    }
    public void CalculateHcost(Vector2 playerpos) //dit is de hcost die wordt berekend wanneer de player een nieuwe positie heeft 
    {
        Enemy enemy = this;
        LevelGrid grid = GameWorld.GetObject("levelgrid") as LevelGrid;
        for (int y = 0; y < 30; y++)
        {
            for (int x = 0; x < 30; x++)
            {
                hcost_grid[x, y] = (float)Vector2.Distance(new Vector2(x, y), playerpos);

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
        }
    }
    void CalculateNeighbours(Node currentNode)
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
    }/// <summary>
     /// //////////////////////9999
     /// </summary>
     /// <param name="pos"></param>
    void Move(Vector2 pos)
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

        if (aiplayerdistance < 2.2f|| die || dead)
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

    bool InRange()
    {
        bool range = false;
        Enemy enemy = this;
        Player player = GameWorld.GetObject("player") as Player;
        float distance = Vector2.Distance(this.GridPos, player.GridPos);

        if (distance < 10)
            range = true;

        return range;
    }

}

