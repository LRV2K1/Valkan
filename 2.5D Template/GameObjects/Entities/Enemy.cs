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

    int[,] hcost_grid = new int[40, 40];
    int hcost_playerX;
    int hcost_playerY;
    bool pathFound = false;
    Path path1 = new Path(); // nieuw path word aangemaakt
    enum AiState { SLEEP, RUNNING } // de ai heeft 2 states waarin hij switched
    AiState currentState = AiState.SLEEP;
    List<Vector2> destinationQueue = new List<Vector2>();
    List<Node> nodes = new List<Node>();
    List<Node> closedNodesList = new List<Node>();

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
            DesCalculate(player.GridPos);
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
    }
    public void Startup()
    {
        Player player = GameWorld.GetObject("player") as Player;
        destinationQueue.Add(player.GridPos); //De StartPositie wordt toegevoegd aan de destinationQueue
        LevelGrid grid = GameWorld.GetObject("tiles") as LevelGrid;
        hcost_playerX = (int)player.GridPos.X;
        hcost_playerY = (int)player.GridPos.Y;
        for (int y = 0; y < 40; y++)
        {
            string line = "";
            for (int x = 0; x < 40; x++)
            {
                hcost_grid[x, y] = Math.Abs(hcost_playerX - x) + Math.Abs(hcost_playerY - y); //de hcostgrid krijgt elk vakje een value, de value is de afstand vanaf het vakje naar de player (destination).

                if (grid.GetTileType(x, y) == TileType.Wall)
                {
                    hcost_grid[x, y] = 999;  //wanneer in de grid van de map een muur staat zal de hcost grid die tellen als een onbruikbare getal
                }
                line += hcost_grid[x, y].ToString() + " , ";

                Vector2 nodepos = new Vector2(x, y);
                Node node = new Node(nodepos, hcost_grid[x, y]); //dehcost wordt toegevoegt aan de Node
                nodes.Add(node); //node wordt toegovoegd aan de lijst van nodes 
            }
            Console.WriteLine(line);
        }
    }
    public void DesCalculate(Vector2 playerpos)
    {
        Enemy enemy = this;
        count++;
        if (destinationQueue.Count() >= 1) //als er een distination  in de lijst staat zal de ai naar daar gaan
        {
            if (destinationQueue[0] == this.GridPos && pathFound) //de ai heeft de player gevonden door de path te gebruiken
            {
                //found
                destinationQueue.RemoveAt(destinationQueue.Count() - 1); //de laatste destination wordt verwijderd
                currentState = AiState.SLEEP;
                pathFound = false;

            }

            else if (destinationQueue[0] == this.GridPos && !pathFound)  //de ai heeft de player gevonden maar zonder de path te gebruiken, dus de player kwam naar de ai
            {
                //found 
                currentState = AiState.SLEEP;
                pathFound = false;

                destinationQueue.RemoveAt(destinationQueue.Count() - 1); //de laatste destination wordt verwijderd

            }
            else if (destinationQueue[0] != this.GridPos && !pathFound) //de ai moet de path naar de player vinden 
            {
                //new
                currentState = AiState.SLEEP;
            }
            else if (destinationQueue[0] != this.GridPos && pathFound) //de ai heeft de path gevonden maar de player nog niet
            {
                //busy
                currentState = AiState.RUNNING;

            }
        }
        switch (currentState)
        {
            case AiState.SLEEP:
                // if (count >= 10)//de ai heeft een delay van 1 sec
                // {
                destinationQueue.Add(playerpos); //de gegeven playerpostitie wordt toegevoegd aan de destinationQueue
                FindPath(destinationQueue[0]); //Findpath word aanroepen om de pad te vinden van de destination dat vooraan de lijst staat
                                               //    }
                break;

            case AiState.RUNNING:
                if (path1.nodesList.Count() > 0) //wanneer de path berekend
                {
                    if (count > 50)
                    {
                        Move(path1.nodesList[0]); //beweeg naar de eerste positie in de list
                        path1.nodesList.RemoveAt(0); // verwijder de positie na het bewegen
                        count = 0;
                    }
                }
                break;
        }

    }
    public void FindPath(Vector2 playerpos)
    {
        Enemy enemy = this;
        if (currentState == AiState.SLEEP)
        {
            closedNodesList.Clear(); //de closedNodesList wordt helemaal leeg gemaakt
            path1.nodesList.Clear(); //de openNodesList wordt helemaal leeg gemaakt
            CalculateHcost(playerpos); //de Hcost wordt berekend wanneer de player een nieuwe positie heeft
            path1.nodesList.Add(CalculateNextPosition(this.GridPos)); //de nodeslist van de path word de volgendepositie hier toegevoegd vanaf de huidige positie

            while (path1.nodesList[path1.nodesList.Count() - 1] != playerpos) //terwijl de laatste positie in de path niet gelijk staat aan de positie van de player zal de path extra posities toevoegen
            {
                path1.nodesList.Add(CalculateNextPosition(path1.nodesList[path1.nodesList.Count() - 1])); //de volgende positie vanaf de laatste positie wordt hier berekend
            }

            Console.WriteLine("Path Count =    " + path1.nodesList.Count());

            pathFound = true;
        }

    }
    public void CalculateHcost(Vector2 playerpos) //dit is de hcost die wordt berekend wanneer de player een nieuwe positie heeft 
    {

        hcost_playerX = (int)playerpos.X;
        hcost_playerY = (int)playerpos.Y;
        LevelGrid grid = GameWorld.GetObject("tiles") as LevelGrid;
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 25; x++)
            {
                hcost_grid[x, y] = Math.Abs(hcost_playerX - x) + Math.Abs(hcost_playerY - y);
                if (grid.GetTileType(x, y) == TileType.Wall)
                {
                    hcost_grid[x, y] = 999;
                }
                Vector2 nodepos = new Vector2(x, y);

                foreach (Node node in nodes)
                {
                    if (node.nodeXY == nodepos)
                    {
                        node.fcost = hcost_grid[x, y];
                    }
                }
            }
        }
    }
    public Vector2 CalculateNextPosition(Vector2 aipos)
    {
        List<Node> openNodesList = new List<Node>();

        foreach (Node n in nodes)
        {
            if (closedNodesList.Any(item => item == n))
            {
                //als de huidige node al in closedNodes zit, dan doe je niks
            }
            else
            {
                if (n.nodeXY == aipos)
                {
                    closedNodesList.Add(n); //de aipos wordt toegevoegd aan de closedNodesList
                }
                //De if-statements hieronder voegen de posities rondom de aipos aan de OpenNodesList
                if (n.nodeXY == new Vector2(aipos.X + 1, aipos.Y + 1))
                {
                    openNodesList.Add(n);
                }
                else if (n.nodeXY == new Vector2(aipos.X - 1, aipos.Y - 1))
                {
                    openNodesList.Add(n);
                }
                else if (n.nodeXY == new Vector2(aipos.X + 1, aipos.Y - 1))
                {
                    openNodesList.Add(n);
                }
                else if (n.nodeXY == new Vector2(aipos.X - 1, aipos.Y + 1))
                {
                    openNodesList.Add(n);
                }
                else if (n.nodeXY == new Vector2(aipos.X + 1, aipos.Y))
                {
                    openNodesList.Add(n);
                }
                else if (n.nodeXY == new Vector2(aipos.X, aipos.Y + 1))
                {
                    openNodesList.Add(n);
                }
                else if (n.nodeXY == new Vector2(aipos.X - 1, aipos.Y))
                {
                    openNodesList.Add(n);
                }
                else if (n.nodeXY == new Vector2(aipos.X, aipos.Y - 1))
                {
                    openNodesList.Add(n);
                }
            }
        }
        openNodesList = openNodesList.OrderBy(n => n.fcost).ToList(); //de OpenNodesList word georderd op fcost dus de optie waar de fcost het laagst is zal de ai als eerst uitvoeren
        if (closedNodesList.Count() > 20)
        {
            closedNodesList.Clear(); //na 20 stappen worden de plekken waar de ai al is geweest uit de closedList gehaald omdat de Ai anders vast loopt
        }
        try
        {
            return openNodesList[0].nodeXY; //we returnen de stap dat het minste fcost heeft
        }
        catch
        {
            Console.WriteLine("error has occurred but idk how"); //er komt een error soms maar ik weet nog niet waardoor deze komt hierdoor heb ik een random positie gegeven aan de ai waardoor de code niet vast loopt
            return new Vector2(20, 18);
        }
    }
    void Move(Vector2 pos)
    {
        Enemy enemy = this;
        LevelGrid grid = GameWorld.GetObject("tiles") as LevelGrid;
        Vector2 movpos = grid.AnchorPosition((int)pos.X, (int)pos.Y);
        this.Position = movpos; //de ai beweegt naar de gewezen positie
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

