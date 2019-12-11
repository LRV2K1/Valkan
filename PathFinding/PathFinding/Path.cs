using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Path
{
    public List<Vector2> nodesList { get; set; }
    public int totalCost { get; set; }
    public Path()
    {
        nodesList = new List<Vector2>();
    }

    
}

