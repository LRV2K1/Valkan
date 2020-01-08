using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class Block : Skill
{
    protected float resettimer;

    public Block(string assetname, float timer = 1f, int damage = 10, MouseButton mouseButton = MouseButton.Right)
        : base(assetname, mouseButton)
    {
        resettimer = timer;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        Player player = parent as Player;
        player.Block = inputHelper.MouseButtonDown(button) && timer.Ready && player.Stamina >= 20 && !player.Blocked;
        if (player.Blocked)
        {
            player.Blocked = false;
            base.Use(resettimer);
        }
    }
}