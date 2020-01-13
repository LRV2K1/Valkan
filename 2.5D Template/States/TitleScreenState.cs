using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the title screen, here you can select start, settings or quit.
class TitleScreenState : GameObjectLibrary
{
    protected Button startButton, settingsButton, exitButton;
    protected bool firstTime = true;
    public TitleScreenState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Logo", 100, "background");
        RootList.Add(titleScreen);
        startButton = new Button("Sprites/Menu/Play_Button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4);
        startButton.Sprite.Size = new Vector2(1,1.5f);
        RootList.Add(startButton);
        settingsButton = new Button("Sprites/Menu/Settings_Button", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 2);
        RootList.Add(settingsButton);
        exitButton = new Button("Sprites/Menu/Quit_Button", 101);
        exitButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4 * 3);
        RootList.Add(exitButton);
    }

    public override void Update(GameTime gameTime)
    {
        if (firstTime)
        {
            MediaPlayer.Volume = 0.7f;
            GameEnvironment.AssetManager.PlayMusic("Soundtracks/Sad");
            firstTime = false;
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("modeSelectionState", 5);
        }
        else if (settingsButton.Pressed)
        { 
            GameEnvironment.ScreenFade.TransitionToScene("settingsState", 5);
        }
        else if (exitButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("exit");
        }
    }

    public override void Reset()
    {
        firstTime = true;
    }

}
