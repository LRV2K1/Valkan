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
    SkillTimer skill1;
    public Hud(GameObjectLibrary gameworld, int layer = 101, string id = "")
        : base(gameworld, layer, id)
    {
        tube1 = new Tube("Sprites/Menu/spr_health");
        tube1.Position = new Vector2(GameEnvironment.Screen.X - 1.5f* tube1.Width, GameEnvironment.Screen.Y - 1 * tube1.Width);
        Add(tube1);
        tube2 = new Tube("Sprites/Menu/spr_stamina");
        tube2.Position = new Vector2(GameEnvironment.Screen.X - 3 * tube1.Width, GameEnvironment.Screen.Y - 1 * tube1.Width);
        Add(tube2);
        skill1 = new SkillTimer("Sprites/Menu/Skills/spr_skill_0");
        skill1.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y - skill1.Width / 2);
        Add(skill1);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Player player = GameWorld.GetObject("player") as Player;
        if (player != null)
        {
            tube1.TargetSize = (float)player.Health / (float)player.MaxHealth;
            tube2.TargetSize = (float)player.Stamina / (float)player.MaxStamina;
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (inputHelper.MouseLeftButtonPressed() && skill1.Ready)
        {
            skill1.Ready = false;
        }
        if (inputHelper.KeyPressed(Keys.I))
        {
            OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
            overlay.SwitchTo("inventory");
        }
    }
}