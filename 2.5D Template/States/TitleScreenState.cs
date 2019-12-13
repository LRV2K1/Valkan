﻿using System;
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
    protected string nextScene;
    public TitleScreenState()
    {
        
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Logo", 100, "background");
        RootList.Add(titleScreen);
        //Add(titleScreen); //Mag niet meer

        startButton = new Button("Sprites/Menu/spr_button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4);
        RootList.Add(startButton);
        //Add(startButton); //Mag niet meer

        settingsButton = new Button("Sprites/Menu/spr_button_intel", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 2);
        RootList.Add(settingsButton);
        //Add(settingsButton); //Mag niet meer

        exitButton = new Button("Sprites/Menu/spr_button_exit", 101);
        exitButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4 * 3);
        RootList.Add(exitButton);
        //Add(exitButton); //Mag niet meer

        nextScene = "firstTime";
    }

    public override void Update(GameTime gameTime)
    {
        if (nextScene == "firstTime")
        {
            //GameEnvironment.ScreenFade.FadeWhite();
            GameEnvironment.AssetManager.PlayMusic("Soundtracks/Sad");
            nextScene = "";
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
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
