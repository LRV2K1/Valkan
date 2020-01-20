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
    protected string[,] skillbuttons;
    protected string[,] skilltext;
    protected SpriteGameObject Selected;
    protected MapSelectionPopUp popup;
    bool timeron;
    float time;
    List<Button> buttonReadyList;
    List<Button> buttonUnreadyList;

    public HostSelectionState()
    {
        GameEnvironment.GameSettingsManager.SetValue("level", "1"); //load level 1 by default
        buttonReadyList = new List<Button>();
        buttonUnreadyList = new List<Button>();
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Menu/Screen2", 100, "background");
        RootList.Add(titleScreen);
        SpriteGameObject lobbyBackground = new SpriteGameObject("Sprites/Menu/Screen2", 101, "lobby");
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

        popup = new MapSelectionPopUp("Sprites/Menu/Screen2", new Vector2(0.5f, 0.78f));
        RootList.Add(popup);
        popup.LoadButtons();
        popup.active = false;

        Selected = new SpriteGameObject("Sprites/Menu/Selected_Button", 101);
        Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        RootList.Add(Selected);

        skillbuttons = new string[3, 3];
        skilltext = new string[3, 3];
        LoadSkillText();
        LoadSkillButtons();

        GameEnvironment.GameSettingsManager.SetValue("character", "Warrior");
    }

    private void LoadSkillText()
    {
        skilltext[0, 0] = "<Left> \n A heavy sword attack to slay all.";
        skilltext[1, 0] = "<Right> \n A big shield to block all damage.";
        skilltext[2, 0] = "<Space> \n A fast dodge to get clear of enemies.";
        skilltext[0, 1] = "<Left> \n A Powerfull ice spell to freeze all opponents.";
        skilltext[1, 1] = "<Right> \n A fireball to blow up all who stand in your way.";
        skilltext[2, 1] = "<Space> \n A strong force shield to block all objects.";
        skilltext[0, 2] = "<Left> \n A slingshot to annoy everybody.";
        skilltext[1, 2] = "<Right> \n A adrinaline dose to boost everybodies speed.";
        skilltext[2, 2] = "<Space> \n A medic kit to heal everybody back up.";
    }

    private void LoadSkillButtons()
    {
        for (int y = 0; y < skillbuttons.GetLength(1); y++)
        {
            int nummer = 0;
            if (y > 0)
            {
                nummer = 6 + (y - 1) * 3;
            }
            SkillHoverButton button = new SkillHoverButton("Sprites/Menu/Skills/spr_skill_" + nummer);
            button.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8, 650);
            button.Visible = false;
            button.Text = skilltext[0, y];
            RootList.Add(button);
            skillbuttons[0, y] = button.Id;

            int nummer2 = 4;
            if (y > 0)
            {
                nummer2 = 7 - (y - 1) * 5;
            }
            SkillHoverButton button2 = new SkillHoverButton("Sprites/Menu/Skills/spr_skill_" + nummer2);
            button2.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 + 90, 650);
            button2.Visible = false;
            button2.Text = skilltext[1, y];
            RootList.Add(button2);
            skillbuttons[1, y] = button2.Id;

            int nummer3 = 5;
            if (y > 0)
            {
                nummer3 = 8 - (y - 1) * 7;
            }
            SkillHoverButton button3 = new SkillHoverButton("Sprites/Menu/Skills/spr_skill_" + nummer3);
            button3.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 + 180, 650);
            button3.Visible = false;
            button3.Text = skilltext[2, y];
            RootList.Add(button3);
            skillbuttons[2, y] = button3.Id;
        }
    }

    public override void Update(GameTime gameTime) //create and remove player buttons
    {
        base.Update(gameTime);
        //if player 2 has connected ---> player2Button.Visible = true;
        //if player 3 has connected ---> player3Button.Visible = true;
        //if player 4 has connected ---> player4Button.Visible = true;
        string playerclass = GameEnvironment.GameSettingsManager.GetValue("character");
        foreach (string button in skillbuttons)
        {
            (GetObject(button) as Button).Visible = false;
        }
        switch (playerclass)
        {
            default:
                break;
            case "Warrior":
                (GetObject(skillbuttons[0, 0]) as Button).Visible = true;
                (GetObject(skillbuttons[1, 0]) as Button).Visible = true;
                (GetObject(skillbuttons[2, 0]) as Button).Visible = true;
                break;
            case "Wizzard":
                (GetObject(skillbuttons[0, 1]) as Button).Visible = true;
                (GetObject(skillbuttons[1, 1]) as Button).Visible = true;
                (GetObject(skillbuttons[2, 1]) as Button).Visible = true;
                break;
            case "Bard":
                (GetObject(skillbuttons[0, 2]) as Button).Visible = true;
                (GetObject(skillbuttons[1, 2]) as Button).Visible = true;
                (GetObject(skillbuttons[2, 2]) as Button).Visible = true;
                break;
        }
        if (timeron) //send world
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 1)
            {
                if (!MultiplayerManager.Party.playerlist.AllReceivedWorld())
                {
                    MultiplayerManager.Party.Send("World Level_" + GameEnvironment.GameSettingsManager.GetValue("level") + " " + MultiplayerManager.Party.WorldToString("Level_" + GameEnvironment.GameSettingsManager.GetValue("level")), 9999);
                }
                else
                {
                    MultiplayerManager.Party.Send("Start", 9999);
                    MultiplayerManager.Party.CreatePlayers();
                    GameEnvironment.ScreenFade.TransitionToScene("playingState");
                    timeron = false;
                }
                time = 0;
            }
        }

        if (MultiplayerManager.Party != null) //if not enough buttons make one
        {
            for (int i = buttonReadyList.Count; i < MultiplayerManager.Party.playerlist.playerlist.Count; i++)
            {
                buttonReadyList.Add(new Button("Sprites/Menu/Ready_Button", 101));
                buttonReadyList[i].Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - 2) / 10 * (i + 2) + 2 * 1.5f);
                buttonReadyList[i].Sprite.Size = new Vector2(1.3f, 2f);
                buttonReadyList[i].Visible = false;
                RootList.Add(buttonReadyList[i]);

                buttonUnreadyList.Add(new Button("Sprites/Menu/Unready_Button", 101));
                buttonUnreadyList[i].Position = new Vector2(GameEnvironment.Screen.X / 20 * 13, (GameEnvironment.Screen.Y - 2) / 10 * (i + 2) + 2 * 1.5f);
                buttonUnreadyList[i].Sprite.Size = new Vector2(1.3f, 2f);
                buttonUnreadyList[i].Visible = true;
                RootList.Add(buttonUnreadyList[i]);
            }

            for (int i = buttonReadyList.Count; i > MultiplayerManager.Party.playerlist.playerlist.Count; i--) //if too many buttons remove one
            {
                buttonReadyList[i - 1].Visible = false;
                buttonReadyList.RemoveAt(i - 1);

                buttonUnreadyList[i - 1].Visible = false;
                buttonUnreadyList.RemoveAt(i - 1);
            }
            for (int  i = 0; i < MultiplayerManager.Party.playerlist.playerlist.Count; i++)
            {
                if (MultiplayerManager.Party.playerlist.playerlist[i].isready)
                {
                    buttonReadyList[i].Visible = true;
                    buttonUnreadyList[i].Visible = false;
                }
                else
                {
                    buttonReadyList[i].Visible = false;
                    buttonUnreadyList[i].Visible = true;
                }
            }
            //Console.WriteLine(buttonList.Count + " c " + MultiplayerManager.Party.playerlist.playerlist.Count);
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
            if (MultiplayerManager.Party.playerlist.AllReady()) //if everyone is ready
            {
                MultiplayerManager.Party.CloseParty();               
                MultiplayerManager.Party.Send("World Level_" + GameEnvironment.GameSettingsManager.GetValue("level") + " " + MultiplayerManager.Party.WorldToString("Level_" + GameEnvironment.GameSettingsManager.GetValue("level")), 9999);
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
            MultiplayerManager.Party.playerlist.Modify(Connection.MyIP(), character: "Warrior");
            MultiplayerManager.Party.Send("Playerlist:" + MultiplayerManager.Party.playerlist.ToString(), 9999);
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (sorcererButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Wizzard");
            MultiplayerManager.Party.playerlist.Modify(Connection.MyIP(), character: "Wizzard");
            MultiplayerManager.Party.Send("Playerlist:" + MultiplayerManager.Party.playerlist.ToString(), 9999);
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 2, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (bardButton.Pressed)
        {
            GameEnvironment.GameSettingsManager.SetValue("character", "Bard");
            MultiplayerManager.Party.playerlist.Modify(Connection.MyIP(), character: "Bard");
            MultiplayerManager.Party.Send("Playerlist:" + MultiplayerManager.Party.playerlist.ToString(), 9999);
            Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 3, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
        }
        else if (returnButton.Pressed)
        {
            if (MultiplayerManager.Party != null)
            {
                MultiplayerManager.Party.Disconnect();
            }
        }
    }

    public override void Reset()
    {
        base.Reset();
        GameEnvironment.GameSettingsManager.SetValue("character", "Warrior");
        Selected.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 8);
    }
}
