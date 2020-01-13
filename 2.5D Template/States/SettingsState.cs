﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

class SettingsState : GameObjectLibrary
{
    protected Button startButton, settingsButton, returnButton;
    protected bool firstTime = true;
    protected bool screen1, screen2;
    public SettingsState()
    {

        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);

        startButton = new Button("Sprites/Menu/spr_button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4);
        RootList.Add(startButton);

        settingsButton = new Button("Sprites/Menu/spr_button_intel", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 2);
        RootList.Add(settingsButton);

        returnButton = new Button("Sprites/Menu/spr_button_exit", 101);
        returnButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - startButton.Height) / 4 * 3);
        RootList.Add(returnButton);
        

    }

    public override void Update(GameTime gameTime)
    {
        if(firstTime)
        {
            GameEnvironment.AssetManager.PlayMusic("Soundtracks/Valkan's Fate - Battle Theme(Garageband)");
            firstTime = false;
            startButton.Active = true;
        }
        if(screen1)
        {
            settingsButton.Active = true;
        }
        if(screen2)
        {
            returnButton.Active = true;
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            screen1 = true;
        }
        else if (settingsButton.Pressed)
        {
            screen2 = true;
        }
        else if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("titleScreen");
        }
    }

    public override void Reset()
    {
        firstTime = true;
    }
}
