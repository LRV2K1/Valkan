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
    protected Button returnButton;
    List<Button> buttonList;
    public PortSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Menu/Screen1", 100, "background");
        RootList.Add(titleScreen);
        SpriteGameObject lobby = new SpriteGameObject("Sprites/Menu/Screen2", 101, "lobby");
        lobby.Sprite.Size = new Vector2(0.75f, 0.5f);
        lobby.Origin = lobby.Sprite.Center;
        lobby.Position = new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2);
        lobby.Sprite.Color = Color.Gray;
        RootList.Add(lobby);
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);

        buttonList = new List<Button>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (MultiplayerManager.Lobby != null)
        {
            for (int i = buttonList.Count; i < MultiplayerManager.Lobby.playerlists.Count; i++)
            {
                buttonList.Add(new Button("Sprites/Menu/Standard_Button", 101));
                buttonList[i].Sprite.Size = new Vector2(1.3f, 2f);
                buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 2 - (buttonList[i].Width * buttonList[i].Sprite.Size.X) / 2, (GameEnvironment.Screen.Y / 3.3f) + (int)(GameEnvironment.Screen.Y / 9) * i);
                RootList.Add(buttonList[i]);
            }

            for (int i = buttonList.Count; i > MultiplayerManager.Lobby.playerlists.Count; i--)
            {
                buttonList[i - 1].Visible = false;
                buttonList.RemoveAt(i - 1);
            }
            //Als je een broadcast hebt ontvangen: ConnectionMade("vul hier het ip in");
            //Als je connection hebt verloren: ConnectionLost("vul hier het ip in");
        }
    }

    private void ClearButtonsList()
    {
        for (int i = buttonList.Count; i > 0; i--)
        {
            buttonList[i - 1].Visible = false;
            buttonList.RemoveAt(i - 1);
            MultiplayerManager.Lobby.portlist.Clear();
            MultiplayerManager.Lobby.playerlists.Clear();
        }
    }
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].Pressed)
            {
                MultiplayerManager.Connect(MultiplayerManager.Lobby.portlist[i]);
                MultiplayerManager.Party.Send("Join", MultiplayerManager.Lobby.portlist[i]);
                ClearButtonsList();
                MultiplayerManager.Lobby.Disconnect();
                GameEnvironment.ScreenFade.TransitionToScene("clientSelectionState", 5);
            }
        }
        if (returnButton.Pressed)
        {
            MultiplayerManager.Lobby.Disconnect();
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
    }
}
