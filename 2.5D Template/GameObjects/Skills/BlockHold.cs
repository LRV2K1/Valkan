using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class BlockHold : Skill
{
    ParticleEffect shield;
    string block_asset;
    float staminatimer;

    public BlockHold(string assetname, string block_asset = "", Keys keys = Keys.Space)
        : base(assetname, MouseButton.None, keys)
    {
        this.block_asset = block_asset;
        staminatimer = 0.1f;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        Player player = parent as Player;
        player.Block = inputHelper.IsKeyDown(key) && player.Stamina >= 2;
        if (player.Block)
        {
            if (timer.Ready)
            {
                player.Stamina-=2;
                base.Use(staminatimer);
                GameEnvironment.AssetManager.PlaySound("SFX/Player/Magic_Shield");
            }
            if (shield == null && block_asset != "")
            {
                shield = new ParticleEffect(block_asset, true);
                shield.Position = GlobalPosition;
                shield.Origin += new Vector2(0, shield.Sprite.Height / 4);
                GameWorld.RootList.Add(shield);
                player.InMovible = true;
            }
        }
        else
        {
            if (shield != null)
            {
                shield.RemoveSelf();
                shield = null;
                player.InMovible = false;
            }

        }
        if (player.Blocked)
        {
            player.Blocked = false;
        }
    }
}