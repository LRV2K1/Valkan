using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Node
{
    public Vector2 nodeXY { get; set; }
    public int fcost { get; set; }
    public Node(Vector2 nodeXY, int fcost)
    {
        this.fcost = fcost;
        this.nodeXY = nodeXY;
    }
}
