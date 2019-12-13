using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

class TitleScreenState : GameObjectList
{
    protected Button startButton, settingsButton, exitButton;
    protected string nextScene;
    public TitleScreenState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Logo", 100, "background");
        Add(titleScreen); //Mag niet meer

        startButton = new Button("Sprites/Menu/spr_button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4);
        Add(startButton); //Mag niet meer

        settingsButton = new Button("Sprites/Menu/spr_button_intel", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 2);
        Add(settingsButton); //Mag niet meer

        exitButton = new Button("Sprites/Menu/spr_button_exit", 101);
        exitButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4 * 3);
        Add(exitButton); //Mag niet meer

        nextScene = "firstTime";
    }

    public override void Update(GameTime gameTime)
    {
        if (nextScene == "firstTime")
        {
            //GameEnvironment.ScreenFade.FadeWhite();
            //GameEnvironment.AssetManager.PlaySong("Sad");
            nextScene = "";
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        /*
        if (GameEnvironment.ScreenFade.FadeToWhite)
        {
            return;
        }
        else if (!GameEnvironment.ScreenFade.FadeToBlack && nextScene == "exit")
        {
            GameEnvironment.QuitGame = true;
        }
        else if (!GameEnvironment.ScreenFade.FadeToBlack && nextScene != "")
        {
            GameEnvironment.GameStateManager.SwitchTo(nextScene);
        }
        */
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            //GameEnvironment.AssetManager.PlaySong("Valkan's Fate - Battle Theme(Garageband)");
            nextScene = "playingState";
            //GameEnvironment.ScreenFade.FadeBlack();
        }
        else if (settingsButton.Pressed)
        {
            //GameEnvironment.AssetManager.PlaySong("Valkan's Fate - Battle Theme(Garageband)");
            //GameEnvironment.ScreenFade.FadeBlack();
        }
        else if (exitButton.Pressed)
        {
            nextScene = "exit";
            //GameEnvironment.ScreenFade.FadeBlack();
        }
    }

}
