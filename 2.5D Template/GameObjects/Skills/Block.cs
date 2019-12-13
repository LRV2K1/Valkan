using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Block : SecondairySkill
{
    protected bool block;
    public Block(string assetname, float time = 1f, int damage = 10)
        : base(assetname, time, damage)
    {
        block = false;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        Player player = parent as Player;
        block = inputHelper.MouseButtonDown(button) && timer.Ready && player.Stamina >= 20;
    }

    public bool Blocking
    {
        get { return block; }
        set 
        {
            block = value;
            if (!block)
            {
                Player player = parent as Player;
                player.Stamina -= 20;
                base.Use(time);
            }
        }
    }
}