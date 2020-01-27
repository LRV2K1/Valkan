using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the state where you can change the settings
class SettingsState : GameObjectLibrary
{
    protected Button fullScreenButton, volumeDownButton, volumeUpButton, returnButton;
    protected bool firstTime = true;
    public SettingsState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Menu/Screen1", 100, "background");
        RootList.Add(titleScreen);
        fullScreenButton = new Button("Sprites/Menu/FullScreen_Button", 101);
        fullScreenButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - fullScreenButton.Width / 2, (GameEnvironment.Screen.Y - fullScreenButton.Height) / 4);
        RootList.Add(fullScreenButton);
        volumeUpButton = new Button("Sprites/Menu/VolumeUp_Button", 101);
        volumeUpButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - volumeUpButton.Width / 2, (GameEnvironment.Screen.Y - fullScreenButton.Height) / 8 * 3);
        RootList.Add(volumeUpButton);
        volumeDownButton = new Button("Sprites/Menu/VolumeDown_Button", 101);
        volumeDownButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - volumeDownButton.Width / 2, (GameEnvironment.Screen.Y - fullScreenButton.Height) / 2);
        RootList.Add(volumeDownButton);
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - volumeDownButton.Width / 2, (GameEnvironment.Screen.Y - fullScreenButton.Height) / 4 * 3);
        RootList.Add(returnButton);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (fullScreenButton.Pressed)
        {
            GameEnvironment.NewChanges = true;
        }
        else if (volumeUpButton.Pressed)
        {
            GameStart.ChangeAudio(0.1f);
        }
        else if (volumeDownButton.Pressed)
        {
            GameStart.ChangeAudio(-0.1f);
        }
        else if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("titleScreen", 5);
        }
    }

    public override void Reset()
    {
        firstTime = true;
    }
}
