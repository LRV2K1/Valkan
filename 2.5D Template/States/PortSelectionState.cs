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
    public PortSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);
        selectButton = new Button("Sprites/Menu/Select_Button", 101);
        selectButton.Position = new Vector2((GameEnvironment.Screen.X - selectButton.Width) / 16 * 13, (GameEnvironment.Screen.Y - selectButton.Height) / 4);
        RootList.Add(selectButton);
        selectButton2 = new Button("Sprites/Menu/Select_Button", 101);
        selectButton2.Position = new Vector2((GameEnvironment.Screen.X - selectButton2.Width) / 16 * 13, (GameEnvironment.Screen.Y - selectButton.Height) / 2);
        RootList.Add(selectButton2);
        returnButton = new Button("Sprites/Menu/Return_Button", 101);
        returnButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - returnButton.Width / 2, (GameEnvironment.Screen.Y - returnButton.Height) / 8 * 7);
        RootList.Add(returnButton);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (selectButton.Pressed && MultiplayerManager.party == null)
        {
            MultiplayerManager.lobby.Disconnect();
            MultiplayerManager.Connect(9999);
            MultiplayerManager.party.Send("Join", 9999); //send to party that we joined
            Console.WriteLine(Connection.MyIP().ToString());
        }
        else if (selectButton2.Pressed)
        {
            
        }
        else if (returnButton.Pressed)
        {
            if (MultiplayerManager.lobby != null)
            {
                MultiplayerManager.lobby.Disconnect();
            }
            GameEnvironment.ScreenFade.TransitionToScene("hostClientSelectionState", 5);
        }
    }
}
