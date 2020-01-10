using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

class TitleScreenState : GameObjectLibrary
{
    protected Button startButton, settingsButton, exitButton;
    protected bool firstTime = true;
    public TitleScreenState()
    {
        
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Logo", 100, "background");
        RootList.Add(titleScreen);

        startButton = new Button("Sprites/Menu/spr_button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4);
        RootList.Add(startButton);

        settingsButton = new Button("Sprites/Menu/spr_button_intel", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 2);
        RootList.Add(settingsButton);

        exitButton = new Button("Sprites/Menu/spr_button_exit", 101);
        exitButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4 * 3);
        RootList.Add(exitButton);
    }

    public override void Update(GameTime gameTime)
    {
        if (firstTime)
        {
            GameEnvironment.AssetManager.PlayMusic("Soundtracks/Sad");
            firstTime = false;
            startButton.Active = true;
            settingsButton.Active = true;
            exitButton.Active = true;
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            //Go To Selection Screen
            GameEnvironment.ScreenFade.TransitionToScene("selection1State");
        }
        else if (settingsButton.Pressed)
        { 
            GameEnvironment.ScreenFade.TransitionToScene("settingsState");
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
