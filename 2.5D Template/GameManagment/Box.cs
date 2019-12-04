using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

public class Box
{
    protected Point a, b, c, d;
    public Box(int ax, int ay, int bx, int by, int cx, int cy, int dx, int dy)
    {
        a = new Point(ax, ay);
        b = new Point(bx, by);
        c = new Point(cx, cy);
        d = new Point(dx, dy);
    }

    public bool Contains(int x, int y)
    {
        return x > a.X && x < c.X && y > b.Y && y < d.Y;
    }

    public bool Intersects(Box b)
    {
        return true;
    }

    public Vector2 Detph()
    {
        return Vector2.Zero;
    }
}

