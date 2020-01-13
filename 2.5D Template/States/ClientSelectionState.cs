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
    protected Button readyButton, warriorButton, sorcererButton, bardButton, returnButton;
    public ClientSelectionState()
    {
        //Load all menu sprites (e.g. background images, overlay images, button sprites)
        SpriteGameObject titleScreen = new SpriteGameObject("Sprites/Overlay/Menu_BG_Grey", 100, "background");
        RootList.Add(titleScreen);
        readyButton = new Button("Sprites/Menu/Ready_Button", 101);
        readyButton.Position = new Vector2((GameEnvironment.Screen.X - readyButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - readyButton.Height) / 6);
        RootList.Add(readyButton);
        warriorButton = new Button("Sprites/Menu/Select_Button", 101);
        warriorButton.Position = new Vector2((GameEnvironment.Screen.X - warriorButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - warriorButton.Height) / 12 * 5);
        RootList.Add(warriorButton);
        sorcererButton = new Button("Sprites/Menu/Select_Button", 101);
        sorcererButton.Position = new Vector2((GameEnvironment.Screen.X - sorcererButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - sorcererButton.Height) / 12 * 6);
        RootList.Add(sorcererButton);
        bardButton = new Button("Sprites/Menu/Select_Button", 101);
        bardButton.Position = new Vector2((GameEnvironment.Screen.X - bardButton.Width) / 8 * 1, (GameEnvironment.Screen.Y - bardButton.Height) / 12 * 7);
        RootList.Add(bardButton);
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
        if (readyButton.Pressed)
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
            GameEnvironment.ScreenFade.TransitionToScene("portSelectionState", 5);
        }
    }
}
