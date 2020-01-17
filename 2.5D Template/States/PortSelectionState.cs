using System;
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
    protected Button selectButton, selectButton2, returnButton;
    List<Button> buttonList;
    List<Button> portList;
    List<string> ipList;
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
        /*selectButton = new Button("Sprites/Menu/Select_Button", 101);
        selectButton.Position = new Vector2((GameEnvironment.Screen.X - selectButton.Width) / 4 * 3, (GameEnvironment.Screen.Y - selectButton.Height) / 5 * 2);
        selectButton.Sprite.Size = new Vector2(0.5f, 0.5f);
        RootList.Add(selectButton);
        selectButton2 = new Button("Sprites/Menu/Select_Button", 101);
        selectButton2.Sprite.Size = new Vector2(0.5f, 0.5f);
        selectButton2.Position = new Vector2((GameEnvironment.Screen.X - selectButton2.Width) / 4 * 3, (GameEnvironment.Screen.Y - selectButton.Height) / 2);
        RootList.Add(selectButton2);*/
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);

        buttonList = new List<Button>();
        portList = new List<Button>();
        ipList = new List<string>();
        LoadButtons();

        //Dit voegt dus en regel toe
        ConnectionMade("123.123.123.123:15099");
        ConnectionMade("123.123.123.123:15099");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //Als je een broadcast hebt ontvangen: ConnectionMade("vul hier het ip in");
        //Als je connection hebt verloren: ConnectionLost("vul hier het ip in");
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].Pressed)
            {
                MultiplayerManager.Connect(9999);
                // Connect with ipList[i] (dit is het ip die connection heeft gemaakt in string vorm)
                //Voor nu heb ik de transition naar ClientSelectionState maar die moet absoluut weg uiteindelijk.
                GameEnvironment.ScreenFade.TransitionToScene("clientSelectionState", 5);
            }
        }
        if (returnButton.Pressed)
        {
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
    }

    public void LoadButtons()
    {
        Vector2 startposition = new Vector2(GameEnvironment.Screen.X / 2 + returnButton.Width, (GameEnvironment.Screen.Y / 13) * 3);
        Vector2 newPosition;
        int yOffset = (int)(GameEnvironment.Screen.Y / 16);
        for (int i = 0; i < 9; i++)
        {
            newPosition = new Vector2(startposition.X, startposition.Y + yOffset * i);
            Button button = new Button("Sprites/Menu/Select_Button", 109);
            buttonList.Add(button);
            button.Sprite.Size = new Vector2(0.6f, 0.6f);
            button.Position = new Vector2(startposition.X, newPosition.Y);
            RootList.Add(button);
            Button port = new Button("Sprites/Menu/Standard_Button", 109);
            portList.Add(port);
            port.Sprite.Size = new Vector2(2.5f,0.6f);
            port.Position = new Vector2(GameEnvironment.Screen.X / 8, newPosition.Y);
            RootList.Add(port);
            buttonList[i].Visible = false;
            portList[i].Visible = false;
        }
    }

    public void ConnectionMade(string ip)
    {
        ipList.Add(ip);
        portList[ipList.Count() - 1].Visible = true;
        //teken ipList[ipList.Count() - 1] over de button en
        buttonList[ipList.Count() - 1].Visible = true;
    }

    public void ConnectionLost(string ip)
    {
        ipList.Remove(ip);
        portList[ipList.Count() - 1].Visible = false;
        //alle getekende IPs eentje naar beneden schuiven en
        buttonList[ipList.Count() - 1].Visible = false;
    }
}
