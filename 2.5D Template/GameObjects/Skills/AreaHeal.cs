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

    public AreaHeal(string assetname, int skill, string heal_asset = "", float timer = 1f, int heal = 10, float range = 400)
        : base(assetname, skill)
    {
        this.heal_asset = heal_asset;
        this.heal = heal;
        this.range = range;
        resettimer = timer;
    }

    public override void Use(float timer = 2)
    {
        GameEnvironment.AssetManager.PlayPartySound("SFX/Player/Bard_Heal");
        base.Use(resettimer);
        Player player = parent as Player;
        player.Stamina -= 20;
        List<string> surroundingentities = player.GetSurroundingEntities();
        HealPlayers(surroundingentities, player.GlobalPosition);
    }

    public override void Button(bool button)
    {
        Player player = parent as Player;
        if (button && timer.Ready && player.Stamina >= 20)
        {
            Use(resettimer);
        }
    }

    private void HealPlayers(List<string> surroundingentities, Vector2 position)
    {
        List<Player> surroundingPlayers = SurroundingPlayers(position, range);
        foreach(Player player in surroundingPlayers)
        {
            player.Health += heal;
            MakeParticle(player.GlobalPosition, heal_asset);
        }
    }
    public override bool Ready
    {
        get
        {
            Player player = parent as Player;
            return timer.Ready && player.Stamina >= 20;
        }
    }
}

