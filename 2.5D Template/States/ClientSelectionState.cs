using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the state where a player joins a host game and can choose a class before saying they're ready.
class ClientSelectionState : GameObjectLibrary
{
    protected Button readyButton, warriorButton, sorcererButton, bardButton, returnButton, hostButton, youButton, player3Button, player4Button;
    protected SpriteGameObject Selected;
    public ClientSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);
        SpriteGameObject lobbyBackground = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 101, "lobby");
        lobbyBackground.Sprite.Color = Color.DarkBlue;
        lobbyBackground.Sprite.Size = new Vector2(0.3f, 0.5f);
        lobbyBackground.Origin = lobbyBackground.Sprite.Center;
        lobbyBackground.Position = new Vector2((GameEnvironment.Screen.X / 10 * 8), GameEnvironment.Screen.Y / 7 * 3);
        RootList.Add(lobbyBackground);
        readyButton = new Button("Sprites/Menu/Ready_Button", 101);
        readyButton.Position = new Vector2((GameEnvironment.Screen.X - readyButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - readyButton.Height) / 6);
        RootList.Add(readyButton);
        warriorButton = new Button("Sprites/Menu/Player_Warrior_Button", 101);
        warriorButton.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 5);
        RootList.Add(warriorButton);
        sorcererButton = new Button("Sprites/Menu/Player_Wizzard_Button", 101);
        sorcererButton.Position = new Vector2((GameEnvironment.Screen.X - sorcererButton.Width) / 8 * 2, (GameEnvironment.Screen.Y - sorcererButton.Height) / 12 * 5);
        RootList.Add(sorcererButton);
        bardButton = new Button("Sprites/Menu/Player_Bard_Button", 101);
        bardButton.Position = new Vector2((GameEnvironment.Screen.X - bardButton.Width) / 8 * 3, (GameEnvironment.Screen.Y - bardButton.Height) / 12 * 5);
        RootList.Add(bardButton);
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);

        hostButton = new Button("Sprites/Menu/Standard_Button", 101);
        hostButton.Sprite.Size = new Vector2(1.3f, 2f);
        hostButton.Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - hostButton.Height) / 10 * 2);
        RootList.Add(hostButton);
        youButton = new Button("Sprites/Menu/Standard_Button", 101);
        youButton.Sprite.Size = new Vector2(1.3f, 2f);
        youButton.Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - hostButton.Height) / 10 * 3 + youButton.Height / 2);
        RootList.Add(youButton);
        player3Button = new Button("Sprites/Menu/Standard_Button", 101);
        player3Button.Sprite.Size = new Vector2(1.3f, 2f);
        player3Button.Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - hostButton.Height) / 10 * 4 + player3Button.Height);
        RootList.Add(player3Button);
        player3Button.Visible = false;
        player4Button = new Button("Sprites/Menu/Standard_Button", 101);
        player4Button.Sprite.Size = new Vector2(1.3f, 2f);
        player4Button.Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - hostButton.Height) / 10 * 5 + player4Button.Height * 1.5f);
        RootList.Add(player4Button);
        player4Button.Visible = false;

        Selected = new SpriteGameObject("Sprites/Menu/Selected_Button", 101);
        Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        RootList.Add(Selected);
    }

    public override void Update(GameTime gameTime)
    {
        if (false) //every update check if we stored the world after receiving so we can start
        {
            GameEnvironment.GameSettingsManager.SetValue("level", "10");
            GameEnvironment.ScreenFade.TransitionToScene("playingState"); //finally switch to playing scene
        }
        base.Update(gameTime);
        //if player 3 has connected ---> player3Button.Visible = true;
        //if player 4 has connected ---> player4Button.Visible = true;

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (readyButton.Pressed)
        {
            MultiplayerManager.party.Send("Ready", 9999);
        }
        else if (warriorButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Warrior");
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (sorcererButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Wizzard");
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 2, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (bardButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Bard");
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 3, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (returnButton.Pressed)
        {
            MultiplayerManager.party.Disconnect();
            GameEnvironment.ScreenFade.TransitionToScene("portSelectionState", 5);
        }
    }
}
