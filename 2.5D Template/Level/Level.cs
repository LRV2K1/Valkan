using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


partial class Level : GameObjectLibrary
{
    public Level(string name)
        : base()
    {
        LoadLevel("Content/Levels/" + name + ".txt");
    }
}