using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Node
{
    public Vector2 position; //gridposition
    public int f_cost; //total cost
    public int g_cost; //distance node start
    public int h_cost; //distance node end

    public Node parent;

    public Node(int h_cost, int g_cost, Node parent, Vector2 position)
    {
        this.h_cost = h_cost;
        this.g_cost = g_cost;
        f_cost = h_cost + g_cost;
        this.parent = parent;
        this.position = position;
    }
}

partial class Enemy: MovingEntity
{
    List<Node> open;
    Dictionary<Vector2, Node> openNodes;
    Dictionary<Vector2, Node> closed;
    Node start;
    bool foundpath = false;

    List<Vector2> path;
    float timer = 0;

    private void PathFinding(Vector2 goal, GameTime gameTime)
    {
        if (foundpath && !PlayerMoved()) //check if moved
        {
            return;
        }
        if (this.timer > 0) //check timer
        {
            this.timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        this.timer = 0.5f;
        foundpath = false;
        start = new Node(0, Distance(goal, gridPos), null, gridPos);
        open = new List<Node>();
        openNodes = new Dictionary<Vector2, Node>();
        closed = new Dictionary<Vector2, Node>();

        AddOpen(start);

        for (int i = 0; i < 250; i++)
        {
            if (open.Count == 0)
            {
                return;
            }
            Node current = open[0];
            ToClosed(current);
            if (current.position == goal)
            {
                MakePath(current);
                foundpath = true;
                return;
            }

            GetNeighbour(current, goal);
        }
    }

    private void GetNeighbour(Node node, Vector2 goal)
    {
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        if (levelGrid == null)
        {
            return;
        }

        for (int x = (int)node.position.X -1; x <= (int)node.position.X + 1; x++)
        {
            for (int y = (int)node.position.Y - 1; y <= (int)node.position.Y + 1; y++)
            {
                if (closed.ContainsKey(new Vector2(x,y)) || levelGrid.GetTileType(x,y) != TileType.Floor || levelGrid.HasWalls(x,y) || levelGrid.HasItem(x,y)) //already in closed or not usable
                {
                    if (x != goal.X || y != goal.Y)
                    {
                        continue;
                    }
                }
                Node neighbour;
                if (!openNodes.ContainsKey(new Vector2(x, y))) //not seen before
                {
                    neighbour = new Node(1000, 1000, node, new Vector2(x, y));
                    AddOpen(neighbour);
                }
                else
                {
                    neighbour = openNodes[new Vector2(x, y)];
                }

                int g_cost = Distance(goal, new Vector2(x,y));
                int h_cost = node.h_cost + 10;
                if (x != (int)node.position.X && y != (int)node.position.Y) //not straight
                {
                    h_cost += 4;
                }
                int f_cost = g_cost + h_cost;

                if (f_cost < neighbour.f_cost) //closer route
                {
                    neighbour.parent = node;
                    neighbour.h_cost = h_cost;
                    neighbour.g_cost = g_cost;
                    neighbour.f_cost = f_cost;
                    RemoveOpen(neighbour);
                    AddOpen(neighbour);
                }
            }
        }
    }

    private int Distance(Vector2 goal, Vector2 start) //calculate distance from start to goal
    {
        int distance = 0;
        int dx = Math.Abs((int)goal.X - (int)start.X);
        int dy = Math.Abs((int)goal.Y - (int)start.Y);
        int dmax = Math.Max(dx, dy) - Math.Min(dx, dy);
        for (int i = 0; i < Math.Min(dx, dy); i++)
        {
            distance += 14;
        }
        for (int i = 0; i < dmax; i++)
        {
            distance += 10;
        }
        return distance;
    }

    private void AddOpen(Node node)
    {
        //LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        //(levelGrid.Get((int)node.position.X, (int)node.position.Y) as Tile).Sprite.Color = Color.Green;
        openNodes.Add(node.position, node);
        for (int i = 0; i < open.Count; i++)
        {
            if (open[i].f_cost > node.f_cost)
            {
                open.Insert(i, node);
                return;
            }
        }
        open.Add(node);
    }

    private void RemoveOpen(Node node)
    {
        openNodes.Remove(node.position);
        open.Remove(node);
    }

    private void ToClosed(Node node)
    {
        //LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        //(levelGrid.Get((int)node.position.X, (int)node.position.Y) as Tile).Sprite.Color = Color.Red;
        closed.Add(node.position, node);
        openNodes.Remove(node.position);
        open.Remove(node);
    }

    private void MakePath(Node node)
    {
        LevelGrid levelGrid = GameWorld.GetObject("levelgrid") as LevelGrid;
        if (levelGrid == null)
        {
            return;
        }
        path = new List<Vector2>();
        Node current = node;
        while (current.parent != null)
        {
            path.Add(levelGrid.AnchorPosition((int)current.position.X, (int)current.position.Y));
            //(levelGrid.Get((int)current.position.X, (int)current.position.Y) as Tile).Sprite.Color = Color.Blue;
            current = current.parent;
            if (current.position == gridPos)
            {
                return;
            }
        }
    }

    private void MovePath(GameTime gameTime)
    {
        if (!foundpath)
        {
            velocity = Vector2.Zero;
            return;
        }
        if (path.Count == 0)
        {
            velocity = Vector2.Zero;
            return;
        }
        Vector2 destination = path[path.Count - 1];
        
        float distance = Vector2.Distance(destination, position);
        if (distance < 50)
        {
            path.RemoveAt(path.Count - 1);
            distance = Vector2.Distance(destination, position);
        }
        float dx = destination.X - position.X;
        float dy = destination.Y - position.Y;
        velocity = new Vector2((dx / distance) * speed, (dy / distance) * speed);
        
    }

    private bool PlayerMoved()
    {
        if (start == null)
        {
            return true;
        }

        Player player = GameWorld.GetObject("player") as Player;
        if (player.GridPos.X < start.position.X - 1 || player.GridPos.X > start.position.X + 1 || player.GridPos.Y < start.position.Y - 1 || player.GridPos.Y > start.position.Y + 1)
        {
            return true;
        }
        return false;
    }
}