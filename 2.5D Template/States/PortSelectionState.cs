﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the state where you can select which host to join.
class PortSelectionState : GameObjectLibrary
{
    protected Button returnButton;
    List<Button> buttonList;
    public PortSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);
        SpriteGameObject lobby = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 101, "lobby");
        lobby.Sprite.Size = new Vector2(0.75f, 0.5f);
        lobby.Origin = lobby.Sprite.Center;
        lobby.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
        lobby.Sprite.Color = Color.Red;
        RootList.Add(lobby);
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);

        buttonList = new List<Button>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (MultiplayerManager.lobby != null)
        {
            for (int i = buttonList.Count; i < MultiplayerManager.lobby.playerlists.Count; i++)
            {
                buttonList.Add(new Button("Sprites/Menu/Standard_Button", 101));
                buttonList[i].Sprite.Size = new Vector2(1.3f, 2f);
                buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 2 - buttonList[i].Width / 2, (GameEnvironment.Screen.Y / 13) * 3 + (int)(GameEnvironment.Screen.Y / 9) * i);
                RootList.Add(buttonList[i]);
            }

            for (int i = buttonList.Count; i > MultiplayerManager.lobby.playerlists.Count; i--)
            {
                buttonList[i - 1].Visible = false;
                buttonList.RemoveAt(i - 1);
            }
            //Als je een broadcast hebt ontvangen: ConnectionMade("vul hier het ip in");
            //Als je connection hebt verloren: ConnectionLost("vul hier het ip in");
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].Pressed)
            {
                MultiplayerManager.Connect(MultiplayerManager.lobby.portlist[i]);
                MultiplayerManager.party.Send("Join", MultiplayerManager.lobby.portlist[i]);
                MultiplayerManager.lobby.Disconnect();
                GameEnvironment.ScreenFade.TransitionToScene("clientSelectionState", 5);
            }
        }
        if (returnButton.Pressed)
        {
            MultiplayerManager.lobby.Disconnect();
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
    }
}
