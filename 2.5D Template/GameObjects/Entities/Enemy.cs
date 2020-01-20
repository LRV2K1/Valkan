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

    protected bool input;

    int count = 0;

    float[,] hcost_grid = new float[200, 200];
    bool pathFound = false;
    // nieuw path word aangemaakt
    enum AiState { SLEEP, RUNNING } // de ai heeft 2 states waarin hij switched
    AiState currentState = AiState.SLEEP;
    Node nodeStart;
    Node nodeEnd;
    List<Node> path = new List<Node>();
    Node[,] nodes = new Node[200, 200];

    List<Node> untestedNodesList = new List<Node>();
    int counter;
    Vector2 currentplayerpos;
    Vector2 oldplayerpos;

    public Enemy(string assetname, int boundingy, int weight = 200, int layer = 0, string id = "")
        : base(boundingy, 40, weight, layer, id)
    {
        selected = false;
        dead = false;

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

        if (die || dead)
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

        if (InRange()) // als de player in bereik is zal de ai bewegen
        {
            Player player = GameWorld.GetObject("player") as Player;
            DesCalculate(player.GridPos);
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

    public void DesCalculate(Vector2 playerpos)
    {
        Enemy enemy = this;
        count++;
        counter++;
        float distance = Vector2.Distance(playerpos, this.GridPos);
        if (distance < 2 && pathFound) //de ai heeft de player gevonden door de path te gebruiken
        {
            //found 
            currentState = AiState.SLEEP;
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

        switch (currentState)
        {
            case AiState.SLEEP:
                if (count >= 10)//de ai heeft een delay van 1 sec
                {
                    FindPath(playerpos); //Findpath word aanroepen om de pad te vinden van de destination dat vooraan de lijst staat
                }
                break;

            case AiState.RUNNING:
                if (path.Count() != 0) //wanneer de path berekend
                {
                    if (counter > 20)
                    {
                        currentplayerpos = new Vector2((int)playerpos.X, (int)playerpos.Y);

                        Move(path[path.Count() - 1].nodeXY); //beweeg naar de eerste positie in de list
                        path.RemoveAt(path.Count() - 1); // verwijder de positie na het bewegen
                        counter = 0;

                        if (currentplayerpos != oldplayerpos)
                        {
                            path.Clear();
                            pathFound = false;
                            FindPath(playerpos);
                            Move(playerpos);
                            counter = 0;
                        }
                        oldplayerpos = currentplayerpos;
                    }
                }
                else // als er geen pad is dan zal hij niet bewegen en opnieuw proberen met de volgende locatie 
                {
                    pathFound = false;
                    currentState = AiState.SLEEP;
                    Move(playerpos);
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
            if (untestedNodesList.Count() == 0) //als er geen nodes zijn die nog getest moeten worden dan stopt de algoritme zonder dat het een pad heeft gevonden
            {
                break;
            }
            nodeCurrent = untestedNodesList[0];        //de node die momenteel word getest is de node vooraan de lijst 
            nodeCurrent.bvisited = true;

            CalculateNeighbours(nodeCurrent);          // bereken de buren van de momentele geteste node
            foreach (Node neighbour in nodeCurrent.neighbours)
            {
                Vector2 distance = new Vector2((float)Math.Abs(nodeStart.nodeXY.X - neighbour.nodeXY.X), (float)Math.Abs(nodeStart.nodeXY.Y - neighbour.nodeXY.Y));
                if (!neighbour.bvisited && !neighbour.obstacle && distance.X < 10 && distance.Y < 10) //als de neigbour niet al getest is en het geen obstacle is dan zal hij in de lijst worden gezet met Nodes die getest zullen worden
                    untestedNodesList.Add(neighbour);
                float possiblyLowerGoal = nodeCurrent.fLocalGoal + Vector2.Distance(nodeCurrent.nodeXY, neighbour.nodeXY);
                if (possiblyLowerGoal < neighbour.fLocalGoal) //als de mogelijke snellere weg kleiner is dan de weg van de neighbour dan zal deze zijn pad veranderen en de kortere kiezen
                {
                    neighbour.parent = nodeCurrent;
                    neighbour.fLocalGoal = possiblyLowerGoal;
                    neighbour.fGlobalGoal = neighbour.fLocalGoal + Vector2.Distance(neighbour.nodeXY, nodeEnd.nodeXY);
                }
            }
        }

        pathFound = true;
        untestedNodesList.Clear(); //alle nodes worden gecleared zodat de ai opnieuw kan berekenen zonder dat er nodes in de lijst te zijn
        AddParentToPath(nodeCurrent); //hier word het pad toegevoegd doormiddel van terugkijken wat de parents zijn van elk node
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

        for (int y = (int)playerpos.Y - 3; y <= (int)playerpos.Y + 3; y++)
        {
            for (int x = (int)playerpos.X - 3; x <= (int)playerpos.X + 3; x++)
            {
                if (x > 0 && y > 0)
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
                        if (node != null)
                            if (node.nodeXY == nodepos)
                            {
                                node.fGlobalGoal = hcost_grid[x, y];
                            }
                    }
                }
            }
        }

    }
    void CalculateNeighbours(Node currentNode)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                //De if-statements hieronder voegen de posities rondom de Node aan de Node
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
    }
    void Move(Vector2 pos)
    {
        input = false;
        Enemy enemy = this;
        LevelGrid grid = GameWorld.GetObject("levelgrid") as LevelGrid;
        Player player = GameWorld.GetObject("player") as Player;
        Vector2 movpos = grid.AnchorPosition((int)pos.X, (int)pos.Y);

        //de ai beweegt naar de gewezen positie
        float dx = movpos.X - this.Position.X;
        float dy = movpos.Y - this.Position.Y;
        float distance = Vector2.Distance(movpos, this.Position);
        float scale = speed / distance;

        float aiplayerdistance = Vector2.Distance(this.GridPos, player.GridPos);

        if (aiplayerdistance < 2.2f)
        {
            this.velocity = Vector2.Zero;
        }
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

    public override void RemoveSelf()
    {
        base.RemoveSelf();
        (GameWorld as Level).EnemyCount--;
    }

}

