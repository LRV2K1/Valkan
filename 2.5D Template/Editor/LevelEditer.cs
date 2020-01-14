using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class LevelEditer : GameObjectLibrary
{
    public LevelEditer(int x, int y, bool load = false, string path = "")
        : base()
    {
        //check for load or new
        if (!load)
        {
            NewLevel(x, y);
        }
        else
        {
            Load(path);
        }
        LoadOverlay();

        Reset();
    }
}