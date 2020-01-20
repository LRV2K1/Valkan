using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class SpeedBuff : Skill
{
    string buf_asset;
    float resettimer;
    float speedmultiplier;
    float speedmultipliertimer;
    float range;

    public SpeedBuff(string assetname, string buf_asset = "", float timer = 4f, float range = 200, float speedmultiplier = 1.4f, float speedmultipliertimer = 2f, MouseButton mouseButton = MouseButton.Right)
        : base(assetname, mouseButton)
    {
        this.buf_asset = buf_asset;
        this.speedmultiplier = speedmultiplier;
        this.speedmultipliertimer = speedmultipliertimer;
        this.range = range;
        resettimer = timer;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        Player player = parent as Player;
        if (inputHelper.MouseButtonPressed(button) && timer.Ready && player.Stamina >= 30)
        {
            Use(resettimer);
        }
    }

    public override void Use(float timer = 2)
    {
        GameEnvironment.AssetManager.PlaySound("SFX/Player/Bard_Speed");
        base.Use(resettimer);
        Player player = parent as Player;
        player.Stamina -= 30;
        List<string> surroundingentities = player.GetSurroundingEntities();
        BuffPlayer(surroundingentities, player.GlobalPosition);
    }

    private void BuffPlayer(List<string> surroundingentities, Vector2 position)
    {
        List<Player> surroundingplayers = SurroundingPlayers(surroundingentities, position, range);
        foreach(Player player in surroundingplayers)
        {
            player.AddSpeedMultiplier(speedmultipliertimer, speedmultiplier);
            MakeParticle(player.GlobalPosition - new Vector2(0, 1), buf_asset);
        }
    }

    public override bool Ready
    {
        get {
            Player player = parent as Player;
            return timer.Ready && player.Stamina >= 30; 
        }
    }
}

