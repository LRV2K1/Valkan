using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

//This is the Online Selection Screen. Here you can choose to play Online 
class PortSelectionState : GameObjectLibrary
{
    protected Button startButton, settingsButton, returnButton;
    protected bool firstTime = true;
    public List<IPAddress> gamelist = new List<IPAddress>();

    public PortSelectionState()
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
        if (startButton.Pressed && MultiplayerManager.party == null)
        {
            MultiplayerManager.lobby.Disconnect();
            MultiplayerManager.Connect(9999);
            MultiplayerManager.party.Send("Join", 9999); //send to party that we joined
        }
        else if (settingsButton.Pressed)
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

    public override void Reset()
    {
        firstTime = true;
    }
    //drawmethod
    //draw gamelist

}
