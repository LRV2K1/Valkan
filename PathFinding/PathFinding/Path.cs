using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Path 
{
    public List<Vector2> nodesList { get; set; } //de path bestaat uit een lijst van nodes
    public Path()
    {
        nodesList = new List<Vector2>();
    }
}

