using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
class AI
{
    Texture2D ai;
    Vector2 aipos;
    Player player;
    Rectangle airect;
    Grid grid;
    int count = 0;
    Random random = new Random();

    int[,] hcost_grid = new int[25, 20];
    int hcost_playerX;
    int hcost_playerY;

    bool codeDone = true;
    bool pathFound = false;
    Path path1 = new Path();
    String aiState = "SLEEP";
    List<Vector2> destinationQueue = new List<Vector2>();

    List<Node> nodes = new List<Node>();
    List<Node> closedNodesList = new List<Node>();

    public AI(Player player, Grid grid)
    {
        aipos = new Vector2(18, 15);
        this.player = player;
        this.grid = grid;
    }
    public void Initialize()
    {
        destinationQueue.Add(player.playerpos);
        for (int y = 0; y < 20; y++)
        {
            string line = "";
            for (int x = 0; x < 25; x++)
            {
                hcost_grid[x, y] = Math.Abs(hcost_playerX - x) + Math.Abs(hcost_playerY - y);
                if (grid.grid[x, y] == 1)
                    hcost_grid[x, y] = 999;
                line += hcost_grid[x, y].ToString() + " , ";

                Vector2 nodepos = new Vector2(x, y);
                Node node = new Node(nodepos, hcost_grid[x, y]);
                nodes.Add(node);
            }
            Console.WriteLine(line);
        }

    }
    public void LoadContent()
    {
        ai = Game1.ContentManager.Load<Texture2D>("ai");

    }

    public void CalculateHcost(Vector2 playerpos)
    {
        hcost_playerX = (int)playerpos.X;
        hcost_playerY = (int)playerpos.Y;
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 25; x++)
            {
                hcost_grid[x, y] = Math.Abs(hcost_playerX - x) + Math.Abs(hcost_playerY - y);
                if (grid.grid[x, y] == 1)
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
    public void Update(Vector2 playerpos)
    {
        count++;
        if (destinationQueue.Count() >= 1)
        {
            if (destinationQueue[0] == aipos && pathFound)
            {
                //found
                destinationQueue.RemoveAt(destinationQueue.Count() - 1);

                aiState = "SLEEP";
                pathFound = false;
                count = 0;
            }
            else if (destinationQueue[0] == aipos && !pathFound)
            {
                //found
                aiState = "SLEEP";
                pathFound = false;
                count = 0;
                destinationQueue.RemoveAt(destinationQueue.Count() - 1);

            }
            else if (destinationQueue[0] != aipos && !pathFound)
            {
                //new
                aiState = "SLEEP";
            }
            else if (destinationQueue[0] != aipos && pathFound)
            {
                //busy
                aiState = "RUNNING";
                count = 0;
            }
        }
        switch (aiState)
        {
            case "SLEEP":

                if (count >= 20)
                {
                    destinationQueue.Add(playerpos);
                    FindPath(destinationQueue[0]);
                }

                break;
            case "RUNNING":
                try
                {
                    codeDone = false;
                    Move(path1.nodesList[0]);
                    path1.nodesList.RemoveAt(0);
                }
                catch
                {
                    aiState = "SLEEP";
                }
                break;
        }
        if (path1.nodesList.Count() == 0)
        {
            codeDone = true;
        }


    }
    public void Draw(SpriteBatch spriteBatch)
    {
        airect = new Rectangle((int)aipos.X * 40, (int)aipos.Y * 40, 40, 40);
        spriteBatch.Draw(ai, airect, Color.White);
    }

    public void FindPath(Vector2 playerpos)
    {
        if (aiState == "SLEEP")
        {
            closedNodesList.Clear();
            path1.nodesList.Clear();
            CalculateHcost(playerpos);
            path1.nodesList.Add(CalculateNextPosition(this.aipos));

            while (path1.nodesList[path1.nodesList.Count() - 1] != playerpos)
            {
                path1.nodesList.Add(CalculateNextPosition(path1.nodesList[path1.nodesList.Count() - 1]));
            }

            Console.WriteLine("Path Count =    " + path1.nodesList.Count());

            pathFound = true;
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
                    closedNodesList.Add(n);
                }
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
        openNodesList = openNodesList.OrderBy(n => n.fcost).ToList();
        if (closedNodesList.Count() > 20)
        {
            closedNodesList.Clear();
        }
        try
        {
            return openNodesList[0].nodeXY;
        }
        catch
        {
            Console.WriteLine("error has occurred but idk how");
            return new Vector2(random.Next(1, 24), random.Next(1, 19));
        }
    }
    void Move(Vector2 pos)
    {
        aipos = pos;
    }
}






