using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the state where the host can select a map and players can join.
class HostSelectionState : GameObjectLibrary
{
    protected Button startButton, changeButton, warriorButton, sorcererButton, bardButton, returnButton, hostButton, player2Button, player3Button, player4Button;
    protected SpriteGameObject Selected;
    protected MapSelectionPopUp popup;
    public HostSelectionState()
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
        startButton = new Button("Sprites/Menu/Start_Button", 101);
        startButton.Position = new Vector2((GameEnvironment.Screen.X - startButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - startButton.Height) / 6);
        RootList.Add(startButton);
        changeButton = new Button("Sprites/Menu/Change_Button", 101);
        changeButton.Position = new Vector2((GameEnvironment.Screen.X - changeButton.Width) / 8, (GameEnvironment.Screen.Y - changeButton.Height) / 4);
        RootList.Add(changeButton);
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
        hostButton.Sprite.Size = new Vector2(1.3f,2f);
        hostButton.Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - hostButton.Height) / 10 * 2);
        RootList.Add(hostButton);
        player2Button = new Button("Sprites/Menu/Standard_Button", 101);
        player2Button.Sprite.Size = new Vector2(1.3f, 2f);
        player2Button.Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - hostButton.Height) / 10 * 3 + player2Button.Height / 2);
        RootList.Add(player2Button);
        player2Button.Visible = false;
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

        popup = new MapSelectionPopUp("Sprites/Overlay/Menu_BG_Grey", new Vector2(0.5f, 0.7f));
        RootList.Add(popup);
        popup.LoadButtons();
        popup.active = false;

        Selected = new SpriteGameObject("Sprites/Menu/Selected_Button", 101);
        Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        RootList.Add(Selected);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //if player 2 has connected ---> player2Button.Visible = true;
        //if player 3 has connected ---> player3Button.Visible = true;
        //if player 4 has connected ---> player4Button.Visible = true;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if(popup.active)
        {
            return;
        }
        if (startButton.Pressed)
        {
            MultiplayerManager.SetupHost();
            GameEnvironment.GameStateManager.AddGameState("playingState", new PlayingState(GameStart.AssetManager.Content)); //create playingstate with a variable world
            GameEnvironment.ScreenFade.TransitionToScene("playingState"); //finally switch to playing scene
        }
        else if (changeButton.Pressed)
        {
            popup.active = true;
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
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
    }
}
