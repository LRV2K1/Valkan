﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the Online Selection Screen. Here you can choose to play Online 
class HostSelectionState : GameObjectLibrary
{
    protected Button startButton, settingsButton, warriorButton, sorcererButton, bardButton, returnButton;
    protected bool firstTime = true;
    public HostSelectionState()
    {

        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);
        //Start Button
        startButton = new Button("Sprites/Menu/Start_Button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - startButton.Height) / 6);
        RootList.Add(startButton);
        //Change Button
        settingsButton = new Button("Sprites/Menu/Change_Button", 101);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width) / 2, (GameEnvironment.Screen.Y - settingsButton.Height) / 3);
        RootList.Add(settingsButton);
        //Select Warrior
        warriorButton = new Button("Sprites/Menu/Select_Button", 101);
        warriorButton.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 7);
        RootList.Add(warriorButton);
        //Select Sorcerer
        sorcererButton = new Button("Sprites/Menu/Select_Button", 101);
        sorcererButton.Position = new Vector2((GameEnvironment.Screen.X - sorcererButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - sorcererButton.Height) / 12 * 8);
        RootList.Add(sorcererButton);
        //Select Bard
        bardButton = new Button("Sprites/Menu/Select_Button", 101);
        bardButton.Position = new Vector2((GameEnvironment.Screen.X - bardButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - bardButton.Height) / 12 * 9);
        RootList.Add(bardButton);
        //Return Button
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);


    }

    public override void Update(GameTime gameTime)
    {
        if (firstTime)
        {
            firstTime = false;
        }
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            MultiplayerManager.SetupHost();
            GameEnvironment.GameStateManager.AddGameState("playingState", new PlayingState(GameStart.AssetManager.Content)); //create playingstate with a variable world
            GameEnvironment.ScreenFade.TransitionToScene("playingState"); //finally switch to playing scene
        }
        else if (settingsButton.Pressed)
        {
            
        }
        else if (warriorButton.Pressed)
        {
            //Player.Job = "warrior";
        }
        else if (sorcererButton.Pressed)
        {
            //Player.Job = "sorcerer";
        }
        else if (bardButton.Pressed)
        {
            //Player.Job = "bard";
        }
        else if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
    }

    public override void Reset()
    {
        firstTime = true;
    }

}
