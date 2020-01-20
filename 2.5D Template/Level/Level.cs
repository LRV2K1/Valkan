using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Level : GameObjectLibrary
{
    //startup of the level
    int enemycount;

    public Level(string name)
        : base()
    {
        enemycount = 0;
        LoadLevel("Content/Levels/Level_" + name + ".txt");
        if (MultiplayerManager.online)
        {
            SetupClient();
        }
        Reset();
    }

    public int EnemyCount
    {
        get { return enemycount; }
        set
        { 
            enemycount = value; 
            if (enemycount <= 0)
            {
                (GetObject("overlay") as OverlayManager).SwitchTo("finish");
            }
        }
    }
}