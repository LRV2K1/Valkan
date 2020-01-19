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
    bool timeron;
    float time;
    List<Button> buttonList;

    public HostSelectionState()
    {
        GameEnvironment.GameSettingsManager.SetValue("level", "1"); //load level 1 by default
        buttonList = new List<Button>();
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

        popup = new MapSelectionPopUp("Sprites/Overlay/Menu_BG_Grey", new Vector2(0.5f, 0.7f));
        RootList.Add(popup);
        popup.LoadButtons();
        popup.active = false;

        Selected = new SpriteGameObject("Sprites/Menu/Selected_Button", 101);
        Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        RootList.Add(Selected);
    }

    public override void Update(GameTime gameTime) //create and remove player buttons
    {
        base.Update(gameTime);
        if (timeron)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 1)
            {
                if (!MultiplayerManager.party.playerlist.AllReceivedWorld())
                {
                    MultiplayerManager.party.Send("World Level_" + GameEnvironment.GameSettingsManager.GetValue("level") + " " + MultiplayerManager.party.WorldToString("Level_" + GameEnvironment.GameSettingsManager.GetValue("level")), 9999);
                }
                else
                {
                    MultiplayerManager.party.Send("Start", 9999);
                    GameEnvironment.ScreenFade.TransitionToScene("playingState");
                    timeron = false;
                }
                time = 0;
            }
        }
        if (MultiplayerManager.party != null)
        {
            for (int i = buttonList.Count; i < MultiplayerManager.party.playerlist.playerlist.Count; i++)
            {
                buttonList.Add(new Button("Sprites/Menu/Standard_Button", 101));
                buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - 2) / 10 * (i + 2) + 2 * 1.5f);
                buttonList[i].Sprite.Size = new Vector2(1.3f, 2f);
                RootList.Add(buttonList[i]);
            }

            for (int i = buttonList.Count; i > MultiplayerManager.party.playerlist.playerlist.Count; i--)
            {
                buttonList[i - 1].Visible = false;
                buttonList.RemoveAt(i - 1);
            }
            //Console.WriteLine(buttonList.Count + " c " + MultiplayerManager.party.playerlist.playerlist.Count);
        }
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
            if (MultiplayerManager.party.playerlist.AllReady()) //if everyone is ready
            {
                MultiplayerManager.party.Send("Closed: " + Connection.MyIP().ToString() + ":" + MultiplayerManager.party.port, 1000);
                MultiplayerManager.party.isopen = false;                
                MultiplayerManager.party.Send("World Level_" + GameEnvironment.GameSettingsManager.GetValue("level") + " " + MultiplayerManager.party.WorldToString("Level_" + GameEnvironment.GameSettingsManager.GetValue("level")), 9999);
                timeron = true;
            }
            else
            {
                Console.WriteLine("Someone is not ready yet");
            }
        }
        else if (changeButton.Pressed)
        {
            popup.active = true;
        }
        else if (warriorButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Warrior");
            MultiplayerManager.party.playerlist.Modify(Connection.MyIP(), character: "Warrior");
            MultiplayerManager.party.Send("Playerlist:" + MultiplayerManager.party.playerlist.ToString(), 9999);
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (sorcererButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Wizzard");
            MultiplayerManager.party.playerlist.Modify(Connection.MyIP(), character: "Wizzard");
            MultiplayerManager.party.Send("Playerlist:" + MultiplayerManager.party.playerlist.ToString(), 9999);
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 2, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (bardButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Bard");
            MultiplayerManager.party.playerlist.Modify(Connection.MyIP(), character: "Bard");
            MultiplayerManager.party.Send("Playerlist:" + MultiplayerManager.party.playerlist.ToString(), 9999);
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 3, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (returnButton.Pressed)
        {
            if (MultiplayerManager.party != null)
            {
                MultiplayerManager.party.Disconnect();
            }
        }
    }
}
