using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


class Hud : Overlay
{
    Tube tube1;
    Tube tube2;
    SpriteGameObject hud;

    public Hud(GameObjectLibrary gameworld, int layer = 101, string id = "")
        : base(gameworld, layer, id)
    {
        hud = new SpriteGameObject("Sprites/Menu/spr_hud", 100);
        hud.Position = new Vector2(0, GameEnvironment.Screen.Y - hud.Sprite.Height);
        Add(hud);

        //add overlay items
        tube1 = new Tube("Sprites/Menu/spr_health");
        tube1.Position = new Vector2(GameEnvironment.Screen.X - tube1.Height, GameEnvironment.Screen.Y - 10 - 3*tube1.Height / 2 - 20);
        Add(tube1);
        tube2 = new Tube("Sprites/Menu/spr_stamina");
        tube2.Position = new Vector2(GameEnvironment.Screen.X - tube2.Height, GameEnvironment.Screen.Y - 10 - tube2.Height / 2);
        Add(tube2);
    }

    public override void Update(GameTime gameTime)
    {
        //update overlay items
        base.Update(gameTime);
        Player player = GameWorld.GetObject("player") as Player;
        if (player != null)
        {
            //check player data and scale tubes
            tube1.TargetSize = (float)player.Health / (float)player.MaxHealth;
            tube2.TargetSize = (float)player.Stamina / (float)player.MaxStamina;
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (inputHelper.KeyPressed(Keys.I))
        {
            OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
            overlay.SwitchTo("inventory");
        }
    }
}