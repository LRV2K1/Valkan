using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class InGameMenu : Overlay
{
    SpriteGameObject background;
    Button resume, main, quit;
    public InGameMenu(GameObjectLibrary gameworld, int layer = 101, string id = "")
        : base(gameworld, layer, id)
    {
        background = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 105);
        background.Origin = background.Sprite.Center;
        background.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
        background.Sprite.Size = new Vector2(0.5f, 0.8f);
        Add(background);
        resume = new Button("Sprites/Menu/Start_Button", 105);
        resume.Origin = resume.Sprite.Center;
        resume.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 4);
        Add(resume);
        main = new Button("Sprites/Menu/Return_Button", 105);
        main.Origin = main.Sprite.Center;
        main.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 4 * 2);
        Add(main);
        quit = new Button("Sprites/Menu/Quit_Button", 105);
        quit.Origin = quit.Sprite.Center;
        quit.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 4 * 3);
        Add(quit);
    }

    public override void Update(GameTime gameTime)
    {

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if(resume.Pressed)
        {
            OverlayManager overlay = GameWorld.GetObject("overlay") as OverlayManager;
            overlay.SwitchTo("hud");
        }
        else if(main.Pressed)
        {
            MultiplayerManager.party.Disconnect();
            GameEnvironment.ScreenFade.TransitionToScene("titleScreen");
        }
        else if (quit.Pressed)
        {
            MultiplayerManager.party.Disconnect();
            GameEnvironment.QuitGame = true;
        }
    }

    public void ActivatePopup()
    {
        this.Visible = true;
    }

}
