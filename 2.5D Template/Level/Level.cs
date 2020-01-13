using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


partial class Level : GameObjectLibrary
{
    //startup of the level
    public Level(string name)
        : base()
    {
        LoadLevel("Content/Levels/" + name + ".txt");
        Reset();
    }
}