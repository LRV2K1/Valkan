using Microsoft.Xna.Framework;
using System.Collections.Generic;
class Node
{
    public bool obstacle { get; set; }
    public bool bvisited { get; set; }
    public Vector2 nodeXY { get; set; } //een node bestaat uit een x-coordinaat en een y-coordinaat
    public float fGlobalGoal { get; set; } //dit is de cost van een node 
    public float fLocalGoal { get; set; }
    public Node parent { get; set; }
    public List<Node> neighbours { get; set; }
    public Node(Vector2 nodeXY, float fGlobalGoal)
    {
        this.bvisited = false;
        this.nodeXY = nodeXY;
        this.fGlobalGoal = fGlobalGoal;
        this.parent = null;
        this.fLocalGoal = 999;
        this.obstacle = false;
        this.neighbours = new List<Node>();
    }

}
