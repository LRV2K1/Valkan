using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Block : SecondairySkill
{
    protected bool block;
    public Block(string assetname, float time = 2f, int damage = 10)
        : base(assetname, time, damage)
    {
        block = false;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        block = inputHelper.MouseButtonDown(button) && timer.Ready;
    }

    public bool Blocking
    {
        get { return block; }
        set 
        { 
            block = value;
            if (!block)
            {
                base.Use(time);
            }
        }
    }
}