using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Node
{
    public Vector2 nodeXY { get; set; } //een node bestaat uit een x-coordinaat en een y-coordinaat
    public int fcost { get; set; } //dit is de cost van een node 
    public Node(Vector2 nodeXY, int fcost)
    {
        this.fcost = fcost;
        this.nodeXY = nodeXY;
    }
}
