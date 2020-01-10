using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class Enemy
{
    private void LoadAnimations(string asset, string animation, int frames, bool looping = true, bool isBackAndForth = false)
    {
        int tempint = 6;
        for (int i = 0; i < 8; i++)
        {
            if (tempint > 7)
            {
                tempint = 0;
            }

            LoadAnimation(asset + "/spr_"+ animation + "_" + tempint + "@" + frames, animation + "_" + i, looping, isBackAndForth);
            tempint += 1;
        }
    }
}
