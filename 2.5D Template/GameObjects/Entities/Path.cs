using System.Collections.Generic;
using Microsoft.Xna.Framework;
class Path 
{
    public List<Vector2> nodesList { get; set; } //de path bestaat uit een lijst van nodes
    public Path()
    {
        nodesList = new List<Vector2>();
    }
}

