using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class AreaHeal : Skill
{
    string heal_asset;
    float resettimer;
    int heal;
    float range;

    public AreaHeal(string assetname, string heal_asset = "", float timer = 1f, int heal = 3, float range = 200f, Keys keys = Keys.Space)
        : base(assetname, MouseButton.None, keys)
    {
        this.heal_asset = heal_asset;
        this.heal = heal;
        this.range = range;
        resettimer = timer;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        Player player = parent as Player;
        if (inputHelper.KeyPressed(key) && timer.Ready && player.Stamina >= 20)
        {
            Use(resettimer);
        }
    }

    public override void Use(float timer = 2)
    {
        base.Use(resettimer);
        Player player = parent as Player;
        player.Stamina -= 20;
        List<string> surroundingentities = player.GetSurroundingEntities();
        HealPlayers(surroundingentities, player.GlobalPosition);
    }

    private void HealPlayers(List<string> surroundingentities, Vector2 position)
    {
        List<Player> surroundingPlayers = SurroundingPlayers(surroundingentities, position, range);
        foreach(Player player in surroundingPlayers)
        {
            player.Health += heal;
            MakeParticle(player.GlobalPosition, heal_asset);
        }
    }
}

